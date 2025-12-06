# %%
%pip install prince

# %%
#libraries
import pandas as pd 
import numpy as np
import pip
import matplotlib.pyplot as plt
import seaborn as sns
import warnings
import os
import scipy.stats as stats
import random
import itertools
import prince

from scipy.stats import shapiro, spearmanr, mannwhitneyu, kruskal
from decimal import Decimal
from sklearn.preprocessing import RobustScaler
# robustscaler
from sklearn.decomposition import PCA

# %% [markdown]
# # Data collectin and initial check

# %% [markdown]
# Load the csv file into a pandas dataframe and check the variable types.

# %%
path = "./dataset_project_eHealth20252026.csv"
dataset = pd.read_csv(path)
dataset.head(5)

# %%
dataset.info()

# %%
df = pd.DataFrame(dataset) #dataframe creation

# %%
print("Elenco colonne del dataset:")
for i, col in enumerate(df.columns):
    print(f"{i+1:02d}: {col}")

# %% [markdown]
# The dataset has 221 rows and 96 columns.

# %% [markdown]
# # Data cleaning

# %% [markdown]
# Check for duplicated rows and delete them.

# %%
df.duplicated() #check for duplicated rows and delete them

# %%
df = df.drop_duplicates()

# %%
df

# %% [markdown]
# The deleted rows are 21, the dataset has 200 rows and 96 columns and it is saved in a new dataset named new_df.

# %%
new_df = df.copy()

# %% [markdown]
# ## Handling missing values
# 
# The dataset contains some missing values. These are not necessarily due to data collection errors — in some cases, they result from the questionnaire structure (skip logic), where certain responses are implicitly determined by previous answers. Therefore, the missing data are assumed to follow a "Missing At Random" (MAR) mechanism rather than being "Missing Completely At Random" (MCAR). 
# However, for analytical purposes, we treat them as completely random, assuming no systematic bias is introduced.
# Imputation Strategy:
# - Categorical variables  -> mode
# - Binary variables (0/1) -> mode
# - Continuous / Likert-scale variables -> mean if |skew| <= 0.5, else median
# - Label columns are excluded from imputation
# 
# This approach helps preserve data consistency while minimizing distortion in the overall distribution. Before imputation, null-like string values are normalized to ensure correct detection and replacement.

# %% [markdown]
# Replace all different representations of missing or empty values in the DataFrame (e.g., empty strings, "NA", "N/A", "nan", "missing", None, etc.) with a consistent NaN value recognized by pandas for easier data cleaning and analysis.

# %%
new_df = new_df.replace(
    ["", " ", "NA", "N/A", "na", "NaN", "nan", "missing", "None", None, pd.NaT, pd.NA],
    np.nan
)

# %%
# helper function to safely get the mode of a series, returning NaN if no mode found
def safe_mode(s):
    m = s.mode(dropna=True)
    return m.iloc[0] if len(m) else np.nan

# 1) define known groups of columns
label_cols = [c for c in new_df.columns if c.endswith("_label") or c.endswith("_labels")]
cat_code_cols = [c for c in ["gender", "education", "marital"] if c in new_df.columns]
binary_cols = [c for c in new_df.columns if c.startswith("dast_")]  # DAST columns are binary 0/1
# automatically add any other columns that are 0/1 and not already classified
for c in new_df.columns:
    if c in binary_cols or c in label_cols:
        continue
    vals = pd.to_numeric(new_df[c], errors="coerce")
    uniq = set(vals.dropna().unique())
    if uniq.issubset({0, 1}) and c not in binary_cols:
        binary_cols.append(c)

# 2) convert columns to numeric where needed (skip labels and categorical coded columns)
numeric_candidates = [c for c in new_df.columns if c not in label_cols]
for c in numeric_candidates:
    new_df[c] = pd.to_numeric(new_df[c], errors="coerce")

# 3) count missing values before imputation
nan_before = new_df.isna().sum().to_dict()
decisions = []

# 4) imputations
# 4a) for categorical coded columns -> fill missing values with mode
for c in cat_code_cols:
    if c in new_df.columns:
        val = safe_mode(new_df[c])
        new_df[c] = new_df[c].fillna(val)
        decisions.append((c, "mode (categorical)"))

# 4b) for binary columns (DAST + other auto-detected 0/1) -> fill missing with mode
for c in binary_cols:
    if c in new_df.columns and c not in cat_code_cols:
        val = safe_mode(new_df[c])
        new_df[c] = new_df[c].fillna(val)
        decisions.append((c, "mode (binary)"))

# 4c) for all other numeric columns -> fill missing with mean or median based on skewness
for c in new_df.columns:
    if c in label_cols or c in cat_code_cols or c in binary_cols:
        continue
    # consider only real numeric columns
    if not pd.api.types.is_numeric_dtype(new_df[c]):
        continue
    s = new_df[c].dropna()
    if s.empty:
        continue
    skew = s.skew()  # Pandas skewness (Fisher-Pearson)
    if np.isfinite(skew) and abs(skew) <= 0.5:
        fill_val = s.mean()
        method = "mean (|skew|<=0.5)"
    else:
        fill_val = s.median()
        method = "median (|skew|>0.5)"
    new_df[c] = new_df[c].fillna(fill_val)
    decisions.append((c, f"{method}; skew={skew:.2f}"))

# 5) count missing values after imputation and calculate how many were replaced
nan_after = new_df.isna().sum().to_dict()
fixed_per_col = {c: int(nan_before.get(c,0) - nan_after.get(c,0)) for c in new_df.columns}

print("=== Imputation decisions (first 30) ===")
for name, how in decisions[:30]:
    print(f"{name:>20} -> {how}")

print("\n=== NaN replaced per column (top 20) ===")
for col, nfix in sorted(fixed_per_col.items(), key=lambda x: x[1], reverse=True)[:20]:
    if nfix > 0:
        print(f"{col:>20}: {nfix}")

print("\nTotale NaN sostituiti:", sum(fixed_per_col.values()))


# %%
new_df.isna().sum()

# %% [markdown]
# The imputation results show that categorical coded columns such as "gender," "education," and "marital" were filled with their mode values, which is a common and effective approach for categorical missing data. Binary columns (mostly dast_1 to dast_10) were also imputed using the mode, reflecting their binary nature and ensuring logical consistency.
# 
# For numeric columns, the code selected the imputation method based on skewness:
# 
# Columns with low skewness (|skew| ≤ 0.5), such as "age," are imputed using the mean.
# 
# Columns with higher skewness (|skew| > 0.5), such as "income," "audit_1" to "audit_10," and "pgsi_1" to "pgsi_5," are imputed using the median. Median imputation is more robust to outliers and skewed distributions, providing more reliable substitution for missing values in such cases.
# 
# The number of missing values replaced per column varies, with some columns having only a few NaNs filled, totaling 73 replacements overall.

# %%
new_df

# %% [markdown]
# # EDA (Exploratory Data Analysis)

# %% [markdown]
# Exploratory Data Analysis (EDA) is a crucial first step in any data analysis or statistical study. EDA involves summarizing and visualizing key characteristics of variables to understand their distributions, detect patterns. In EDA, it is essential to distinguish between continuous and categorical variables, as each type requires different analytical strategies and visualizations. 
# - Continuous variables (such as age and income) are summarized and explored using histograms and normality tests (e.g., Shapiro-Wilk) to assess distribution characteristics. 
# - In contrast, categorical variables (such as gender, education, and marital status) are described with frequency counts and visualized using count plots or bar charts. Normality tests are not applicable to categorical variables, as these codes do not represent continuous measurements and must not be interpreted as such.
# 
# **Shapiro-Wilk test**
# 
# Shapiro-Wilk test is a hypothesis test that evaluates whether a data set is normally distributed. It evaluates data from a sample with the null hypothesis that the data set is normally distributed. A large p-value indicates the data set is normally distributed, a low p-value indicates that it isn’t normally distributed.
# 
# It’s a widely-used statistical tool that can help us find an answer to the normality check we need, but it has one flaw: It doesn’t work well with large data sets. The maximum allowed size for a data set depends on the implementation. For example, for samples larger than 5,000, the Shapiro-Wilk test with SciPy in Python still runs, but it issues a warning that the p-value may not be accurate due to limitations in the test’s underlying assumptions.
# 
# https://builtin.com/data-science/shapiro-wilk-test

# %% [markdown]
# # Univariate EDA

# %% [markdown]
# ## Categorical variables

# %% [markdown]
# ### Gender
# Gender is a categorical variable represented by numeric codes (0, 1, 2, 3) that correspond to ordered categories.

# %%
#[0] Male [1] Female [2] Non-binary [3] Prefer not to say

# Replace numeric codes with labels
gender_labels = {
    0: 'Male',
    1: 'Female',
    2: 'Non-binary',
    3: 'Prefer not to say'
}
new_df['gender_label'] = new_df['gender'].map(gender_labels)

# Plot
plt.figure(figsize=(7, 4))
sns.countplot(data=new_df, x='gender_label', color='skyblue',edgecolor='black')
plt.xlabel('Gender')
plt.ylabel('Subjects')
plt.title('Gender Distribution')
#plt.xticks(rotation=15)
plt.show()

# Print frequency counts for each gender
print("Gender counts:")
print(new_df['gender_label'].value_counts(dropna=False))

# %% [markdown]
# The most represented categories are Female (85) and Male (83): this suggests that outcomes related to gender can be compared between these two groups with reasonable statistical power

# %% [markdown]
# ### Education
# Education is a categorical variable represented by numeric codes (5, 8, 13, 18, 22, 25) that correspond to ordered categories.

# %%
#[5] Elementary school [8] Middle school [13] High School [18]Bachelor's Degree [22] Master's Degree [25] Doctoral Degree

# Replace numeric codes with labels
school_labels = {
    5: 'Elementary',
    8: 'Middle',
    13: 'High',
    18: 'Bachelor',
    22: 'Master',
    25: 'Doctoral'
}
new_df['school_labels'] = new_df['education'].map(school_labels)

# Plot
plt.figure(figsize=(7, 4))
sns.countplot(data=new_df, x='school_labels', color='skyblue',edgecolor='black')
plt.xlabel('Education')
plt.ylabel('Subjects')
plt.title('Education Distribution')
#plt.xticks(rotation=15)
plt.show()

# Print frequency counts for each education level
print("Education counts:")
print(new_df['school_labels'].value_counts(dropna=False))

# %% [markdown]
# This distribution suggests that most participants completed either secondary (High School/Middle School) or tertiary (Bachelor’s/Master’s) education, with fewer achieving only an elementary education or the most advanced level. 
# 
# The presence of all education levels supports adequately powered comparisons for group-level analysis, although the Doctoral category has a limited sample size.

# %% [markdown]
# ### Marital status
# Marital status is a categorical variable represented by numeric codes (0, 1, 2, 3, 4, 5) that correspond to ordered categories.

# %%
# [0] Single [1] Married [2] Divorced [3] Widowed [4] Separated [5] Prefer not to say

# Replace numeric codes with labels
marital_labels = {
    0: 'Single',
    1: 'Married',
    2: 'Divorced',
    3: 'Widowed',
    4: 'Separated',
    5: 'Prefer not to say'
}
new_df['marital_labels'] = new_df['marital'].map(marital_labels)

# Plot
plt.figure(figsize=(7, 4))
sns.countplot(data=new_df, x='marital_labels', color='skyblue',edgecolor='black')
plt.xlabel('Marital')
plt.ylabel('Subjects')
plt.title('Marital Status Distribution')
#plt.xticks(rotation=15)
plt.show()

# Print category counts
print("Marital status counts:")
print(new_df['marital_labels'].value_counts(dropna=False))

# %% [markdown]
# The largest group is Married (81), followed by Single (51), providing a strong basis for comparisons within these groups. 
# The Divorced, Widowed, Separated, and Prefer not to say categories are less frequent, which may limit subgroup analysis but still contribute valuable diversity and context.

# %% [markdown]
# ## Numeric variables

# %% [markdown]
# ### Age

# %%
plt.figure(figsize=(7, 4))
plt.hist(new_df['age'], color='skyblue', edgecolor='black')
plt.xlabel('Age')
plt.ylabel('Subjects')
plt.title('Age Distribution')
plt.show()

stat, p_value = shapiro(new_df['age'])

print(f"Shapiro-Wilk test statistic: {stat:.4f}")
print(f"P-value: {p_value:.4f}")

if p_value > 0.05:
    print("The age distribution is compatible with normality.")
else:
    print("The age distribution is NOT normal.")

# %% [markdown]
# There are noticeable peaks at both ends of the range, with higher counts for subjects around ages 18–20 and 39–40. The lowest frequencies occur in the mid-thirties (about 35–38)

# %% [markdown]
# ### Income

# %%
plt.figure(figsize=(7, 4))
plt.hist(new_df['income'], color='skyblue', edgecolor='black')
plt.xlabel('Income')
plt.ylabel('Subjects')
plt.title('Income Distribution')
plt.show()

stat, p_value = shapiro(new_df['income'])

print(f"Shapiro-Wilk test statistic: {stat:.4f}")
print(f"P-value: {p_value:.4f}")

if p_value > 0.05:
    print("The income distribution is compatible with normality.")
else:
    print("The income distribution is NOT normal.")

# %% [markdown]
# The distribution is clearly not normal, as indicated by the strong skewness and large number of low-income subjects compared to those with higher incomes.
# 
# The Shapiro-Wilk test statistic of 0.8894 and a p-value of 0.0000 confirm this visual impression, providing strong evidence against normality.

# %% [markdown]
# ### Psychometric tests

# %% [markdown]
# **Cronbach’s Alpha**
# 
# Alpha was developed by Lee Cronbach in 195111 to provide a measure of the internal consistency of a test or scale; it is expressed as a number between 0 and 1. Internal consistency describes the extent to which all the items in a test measure the same concept or construct and hence it is connected to the inter-relatedness of the items within the test. 
# If the Alpha is high (above 0.7 or 0.8), it means all the questions are basically measuring the same thing, so you can trust the sum or average as a reliable score for that concept.
# https://pmc.ncbi.nlm.nih.gov/articles/PMC4205511/
# https://how.dev/answers/how-to-implement-cronbachs-alpha-for-reliability-in-python

# %%
def cronbach_alpha(data):
    # Transform the data frame into a correlation matrix
    df_corr = data.corr()
    
    # Calculate N
    # The number of variables is equal to the number of columns in the dataframe
    N = data.shape[1]
    
    # Calculate r
    # For this, we'll loop through all the columns and append every relevant correlation to an array called 'r_s'. 
    # Then, we'll calculate the mean of 'r_s'.
    rs = np.array([])
    for i, col in enumerate(df_corr.columns):
        sum_ = df_corr[col][i+1:].values
        rs = np.append(sum_, rs)
    mean_r = np.mean(rs)
    
   # Use the formula to calculate Cronbach's Alpha 
    cronbach_alpha = (N * mean_r) / (1 + (N - 1) * mean_r)
    return cronbach_alpha

# %%
# Make a copy to work on
df_simplified = new_df.copy()

# %% [markdown]
# #### IAT

# %%
# Select only IAT columns
iat_item_cols = [f"iat_{i}" for i in range(1, 21)]
df_iat = df_simplified[iat_item_cols]

# Plot
n_items = len(iat_item_cols)
n_rows, n_cols = 4, 5

fig, axes = plt.subplots(n_rows, n_cols, figsize=(18, 12))
axes = axes.flatten()
# Histograms for each item
for idx, col in enumerate(iat_item_cols):
    axes[idx].hist(df_simplified[col], bins=6, color='skyblue', edgecolor='black')
    axes[idx].set_title(f"{col}")
    axes[idx].set_xlabel("Response")
    axes[idx].set_ylabel("Subjects")
# Hide unused axes if any
for ax in axes[n_items:]:
    ax.set_visible(False)

plt.tight_layout()
plt.suptitle("Histograms of IAT Item Responses", fontsize=16, y=1.02)
plt.show()

# %%
df_iat.describe()

# %%
c_alpha_iat = cronbach_alpha(df_iat)
print(f"Cronbach_alpha value: {c_alpha_iat:.4f}")

# %% [markdown]
# Most items show broad usage of the full response scale, with medians generally between 2 and 4, and similar variability across the scale. 
# The item-wise histograms reveal that although most items capture a full range of participant responses, some items display higher or lower frequencies at different response categories, highlighting unique response patterns.
# 
# The Cronbach’s alpha for the IAT in this sample is extremely high (α = 0.97), confirming excellent internal consistency and justifying use of the total score for further analysis.

# %%
# Compute the total score
df_simplified['iat_total'] = df_simplified[iat_item_cols].sum(axis=1)

# Visualize histogram of total score
plt.figure(figsize=(7, 4))
plt.hist(df_simplified['iat_total'], bins=15, color='skyblue', edgecolor='black')
plt.xlabel("IAT Total Score")
plt.ylabel("Subjects")
plt.title("Distribution of IAT Total Score")
plt.show()

# Visualize boxplot of total score
plt.figure(figsize=(5, 3))
sns.boxplot(x=df_simplified['iat_total'], color='skyblue')
plt.xlabel("IAT Total Score")
plt.title("Boxplot of IAT Total Score")
plt.show()

# Summary statistics
print("IAT total score summary statistics:")
print(df_simplified['iat_total'].describe())

# Shapiro-Wilk test for normality
stat, p_value = shapiro(df_simplified['iat_total'])
print(f"Shapiro-Wilk test statistic: {stat:.4f}")
print(f"P-value: {p_value:.4f}")

if p_value > 0.05:
    print("The IAT total score distribution is compatible with normality.")
else:
    print("The IAT total score distribution significantly deviates from normality.")

# Drop all IAT item columns from the new DataFrame
df_simplified = df_simplified.drop(columns=iat_item_cols)

# %% [markdown]
# The IAT total score demonstrates a wide range in your sample, with scores spanning from 0 to 100 and a mean of 52.6. The boxplot and histogram illustrate a non-normal, multimodal distribution—confirmed by the Shapiro-Wilk test (statistic = 0.95, p < 0.0001). Given the significant deviation from normality, non-parametric statistical methods are recommended for subsequent analyses involving the IAT total score.

# %% [markdown]
# Categories (Young, 1998; validation studies):
# 
# - Normal/Average user: IAT total < 40
# - Problematic user: 40 ≤ IAT total < 70
# - Severely problematic user: IAT total ≥ 70​

# %%
categories = [
    "Normal" if score < 40 else
    "Problematic" if score < 70 else
    "Severely problematic"
    for score in df_simplified['iat_total']
]

plt.figure(figsize=(7, 5))
sns.countplot(x=pd.Series(categories), palette='Blues', edgecolor='black')
plt.xlabel('IAT Addiction Level')
plt.ylabel('Subjects')
plt.title('Distribution of IAT Addiction Levels')
plt.show()

# Print the counts for each category
print(pd.Series(categories).value_counts())

# %% [markdown]
# #### AUDIT

# %%
# Select only AUDIT columns
audit_item_cols = [f"audit_{i}" for i in range(1, 11)]
df_audit = df_simplified[audit_item_cols]  # or new_df if not already simplified

# Plot
n_items = len(audit_item_cols)
n_rows, n_cols = 2, 5  # 10 items: 2 rows x 5 columns grid

fig, axes = plt.subplots(n_rows, n_cols, figsize=(18, 7))
axes = axes.flatten()
# Histograms for each AUDIT item
for idx, col in enumerate(audit_item_cols):
    axes[idx].hist(df_audit[col], bins=6, color='skyblue', edgecolor='black')
    axes[idx].set_title(f"{col}")
    axes[idx].set_xlabel("Response")
    axes[idx].set_ylabel("Subjects")
# Hide unused axes if any
for ax in axes[n_items:]:
    ax.set_visible(False)

plt.tight_layout()
plt.suptitle("Histograms of AUDIT Item Responses", fontsize=16, y=1.02)
plt.show()


# %%
df_audit.describe()

# %%
c_alpha_audit = cronbach_alpha(df_audit)
print(f"Cronbach_alpha value: {c_alpha_audit:.4f}")

# %% [markdown]
# AUDIT items show a full range of responses, with means clustered around 1 and moderate variability. Most medians are 1 or less, consistent with population-level drinking patterns. The Cronbach's alpha is 0.91, indicating high internal consistency and supporting the use of the total score for further analysis.

# %%
# Compute the total AUDIT score
df_simplified['audit_total'] = df_simplified[audit_item_cols].sum(axis=1)

# Visualize histogram of AUDIT total score
plt.figure(figsize=(7, 4))
plt.hist(df_simplified['audit_total'], bins=15, color='skyblue', edgecolor='black')
plt.xlabel("AUDIT Total Score")
plt.ylabel("Subjects")
plt.title("Distribution of AUDIT Total Score")
plt.show()

# Visualize boxplot of AUDIT total score
plt.figure(figsize=(5, 3))
sns.boxplot(x=df_simplified['audit_total'], color='skyblue')
plt.xlabel("AUDIT Total Score")
plt.title("Boxplot of AUDIT Total Score")
plt.show()

# Summary statistics
print("AUDIT total score summary statistics:")
print(df_simplified['audit_total'].describe())

# Shapiro-Wilk test for normality
stat, p_value = shapiro(df_simplified['audit_total'])
print(f"Shapiro-Wilk test statistic: {stat:.4f}")
print(f"P-value: {p_value:.4f}")

if p_value > 0.05:
    print("The AUDIT total score distribution is compatible with normality.")
else:
    print("The AUDIT total score distribution significantly deviates from normality.")

# Drop all AUDIT item columns from the DataFrame
df_simplified = df_simplified.drop(columns=audit_item_cols)

# %% [markdown]
# The AUDIT total score demonstrates a wide range in your sample, with scores spanning from 0 to 35 and a mean of 10.3. The boxplot and histogram show a strongly right-skewed, non-normal distribution—which is confirmed by the Shapiro-Wilk test (statistic = 0.86, p < 0.0001). Given this significant deviation from normality, non-parametric statistical methods are recommended for analyses involving the AUDIT total score.

# %% [markdown]
# For the AUDIT scale, the most commonly used cut-offs are:
# - Low risk: AUDIT total < 8
# - Hazardous/harmful: 8 ≤ AUDIT total < 16
# - Possible dependence: AUDIT total ≥ 16

# %%
categories = [
    "Low risk" if score < 8 else
    "Hazardous/harmful" if score < 16 else
    "Possible dependence"
    for score in df_simplified['audit_total']
]

categories_series = pd.Series(categories, name='AUDIT Risk Level')

plt.figure(figsize=(7, 5))
sns.countplot(x=categories_series, palette=sns.color_palette("Blues", n_colors=3), edgecolor='black')
plt.xlabel('Subjects')
plt.ylabel('AUDIT Risk Level')
plt.title('Distribution of AUDIT Risk Categories')
plt.show()

# Print the counts for each category
print(pd.Series(categories).value_counts())


# %% [markdown]
# #### DAST

# %%
# Select only DAST-10 item columns
dast_item_cols = [f"dast_{i}" for i in range(1, 11)]
df_dast = df_simplified[dast_item_cols]  # or new_df if not already simplified

# Plot setup
n_items = len(dast_item_cols)
n_rows, n_cols = 2, 5  # 10 items: 2 rows x 5 columns grid

fig, axes = plt.subplots(n_rows, n_cols, figsize=(18, 7))
axes = axes.flatten()

# Histograms for each DAST item
for idx, col in enumerate(dast_item_cols):
    axes[idx].hist(df_dast[col], bins=2, color='skyblue', edgecolor='black')  # DAST items are 0/1
    axes[idx].set_title(f"{col}")
    axes[idx].set_xlabel("Response")
    axes[idx].set_ylabel("Subjects")
# Hide unused axes if any
for ax in axes[n_items:]:
    ax.set_visible(False)

plt.tight_layout()
plt.suptitle("Histograms of DAST-10 Item Responses", fontsize=16, y=1.02)
plt.show()


# %%
df_dast.describe()

# %%
c_alpha_dast = cronbach_alpha(df_dast)
print(f"Cronbach_alpha value: {c_alpha_audit:.4f}")

# %% [markdown]
# DAST-10 items show balanced use of the binary response options, with means around 0.3–0.37 and interquartile ranges from 0 to 1 for all items. This indicates that each question contributes information about the presence or absence of drug-related problems across participants. The Cronbach’s alpha of 0.91 demonstrates excellent internal consistency, consistent with published DAST-10 psychometric studies, and supports using the summed DAST-10 total score as a reliable indicator of drug abuse severity in subsequent analyses.

# %%
# Compute the total DAST score
df_simplified['dast_total'] = df_simplified[dast_item_cols].sum(axis=1)

# Visualize histogram of DAST total score
plt.figure(figsize=(7, 4))
plt.hist(df_simplified['dast_total'], bins=15, color='skyblue', edgecolor='black')
plt.xlabel("DAST Total Score")
plt.ylabel("Subjects")
plt.title("Distribution of DAST Total Score")
plt.show()

# Visualize boxplot of DAST total score
plt.figure(figsize=(5, 3))
sns.boxplot(x=df_simplified['dast_total'], color='skyblue')
plt.xlabel("DAST Total Score")
plt.title("Boxplot of DAST Total Score")
plt.show()

# Summary statistics
print("DAST total score summary statistics:")
print(df_simplified['dast_total'].describe())

# Shapiro-Wilk test for normality
stat, p_value = shapiro(df_simplified['dast_total'])
print(f"Shapiro-Wilk test statistic: {stat:.4f}")
print(f"P-value: {p_value:.4f}")

if p_value > 0.05:
    print("The DAST total score distribution is compatible with normality.")
else:
    print("The DAST total score distribution significantly deviates from normality.")

# Drop all DAST item columns from the DataFrame
df_simplified = df_simplified.drop(columns=dast_item_cols)

# %% [markdown]
# The DAST total score in your sample ranges from 0 to 10 with a mean of 3.4, indicating generally low-to-moderate levels of drug-related problems. The histogram and boxplot reveal a markedly right-skewed distribution, with many participants scoring near 0 and fewer at higher scores. This visual pattern is confirmed by the Shapiro–Wilk test (statistic = 0.89, p < 0.0001), showing a clear deviation from normality; therefore, non-parametric methods or categorical risk levels are recommended for analyses involving the DAST total score.

# %% [markdown]
# Based on the DAST‑10 literature, a common categorization is:​
# - 0: No problems
# - 1–2: Low level
# - 3–5: Moderate level
# - ≥6: Severe level

# %%
# Create DAST risk categories from total score (0–10)
dast_categories = [
    "No problems" if score == 0 else
    "Low level" if score <= 2 else
    "Moderate level" if score <= 5 else
    "Severe level"
    for score in df_simplified['dast_total']
]

dast_cat_series = pd.Series(dast_categories, name='DAST Risk Level')

plt.figure(figsize=(7, 5))
sns.countplot(
    x=dast_cat_series,
    palette=sns.color_palette("Blues", n_colors=4),
    edgecolor='black'
)
plt.xlabel('DAST Risk Level')
plt.ylabel('Subjects')
plt.title('Distribution of DAST-10 Risk Categories')
plt.show()

print(dast_cat_series.value_counts())


# %% [markdown]
# #### PGSI

# %%
# Select only PGSI item columns (assuming pgsi_1 ... pgsi_9)
pgsi_item_cols = [f"pgsi_{i}" for i in range(1, 10)]
df_pgsi = df_simplified[pgsi_item_cols]  # or new_df if not simplified

# Plot setup
n_items = len(pgsi_item_cols)
n_rows, n_cols = 3, 3  # 9 items: 3 rows x 3 columns grid

fig, axes = plt.subplots(n_rows, n_cols, figsize=(15, 9))
axes = axes.flatten()

# Histograms for each PGSI item
for idx, col in enumerate(pgsi_item_cols):
    axes[idx].hist(df_pgsi[col], bins=4, color='skyblue', edgecolor='black')  # responses 0–3
    axes[idx].set_title(col)
    axes[idx].set_xlabel("Response")
    axes[idx].set_ylabel("Subjects")

# Hide unused axes if any
for ax in axes[n_items:]:
    ax.set_visible(False)

plt.tight_layout()
plt.suptitle("Histograms of PGSI Item Responses", fontsize=16, y=1.02)
plt.show()


# %%
df_pgsi.describe()

# %%
c_alpha_pgsi = cronbach_alpha(df_pgsi)
print(f"Cronbach_alpha value: {c_alpha_audit:.4f}")

# %% [markdown]
# The PGSI items display the expected skewed pattern for problem gambling screens: most responses cluster at 0, with progressively fewer endorsements of higher categories (1–3). Item means are all below 1, and quartiles show that at least half of the sample scores 0 on every item, indicating that gambling-related problems are relatively infrequent in this dataset. The Cronbach’s alpha of 0.91 reflects excellent internal consistency, consistent with published PGSI psychometric studies, supporting the use of the summed PGSI total score as a reliable measure of gambling problem severity in subsequent analyses.​

# %%
# Compute the total PGSI score
df_simplified['pgsi_total'] = df_simplified[pgsi_item_cols].sum(axis=1)

# Visualize histogram of PGSI total score
plt.figure(figsize=(7, 4))
plt.hist(df_simplified['pgsi_total'], bins=15, color='skyblue', edgecolor='black')
plt.xlabel("PGSI Total Score")
plt.ylabel("Subjects")
plt.title("Distribution of PGSI Total Score")
plt.show()

# Visualize boxplot of PGSI total score
plt.figure(figsize=(5, 3))
sns.boxplot(x=df_simplified['pgsi_total'], color='skyblue')
plt.xlabel("PGSI Total Score")
plt.title("Boxplot of PGSI Total Score")
plt.show()

# Summary statistics
print("PGSI total score summary statistics:")
print(df_simplified['pgsi_total'].describe())

# Shapiro-Wilk test for normality
stat, p_value = shapiro(df_simplified['pgsi_total'])
print(f"Shapiro-Wilk test statistic: {stat:.4f}")
print(f"P-value: {p_value:.4f}")

if p_value > 0.05:
    print("The PGSI total score distribution is compatible with normality.")
else:
    print("The PGSI total score distribution significantly deviates from normality.")

# Drop all PGSI item columns from the DataFrame
df_simplified = df_simplified.drop(columns=pgsi_item_cols)

# %% [markdown]
# The PGSI total score shows a broad range from 0 to 27, with a mean of 6.2 and an interquartile range roughly between 1 and 7.3. The histogram and boxplot indicate a strongly right-skewed distribution with many low scores and a tail of higher scores, including several outliers at the upper end. The Shapiro–Wilk test (statistic = 0.81, p < 0.0001) confirms a clear deviation from normality, so non-parametric methods or PGSI risk categories should be used for subsequent analyses involving this total score.

# %% [markdown]
# For PGSI, standard risk categories are:
# - 0: Non‑problem gambling
# - 1–2: Low risk
# - 3–7: Moderate risk
# - ≥8: Problem gambling

# %%
# Create PGSI risk categories from total score (0–27)
pgsi_categories = [
    "Non-problem" if score == 0 else
    "Low risk" if score <= 2 else
    "Moderate risk" if score <= 7 else
    "Problem gambling"
    for score in df_simplified['pgsi_total']
]

pgsi_cat_series = pd.Series(pgsi_categories, name='PGSI Risk Level')

plt.figure(figsize=(7, 5))
sns.countplot(
    x=pgsi_cat_series,
    palette=sns.color_palette("Blues", n_colors=4),
    edgecolor='black'
)
plt.xlabel('PGSI Risk Level')
plt.ylabel('Subjects')
plt.title('Distribution of PGSI Risk Categories')
plt.show()

print(pgsi_cat_series.value_counts())

# %% [markdown]
# #### PCL5

# %%
# Select only PCL-5 item columns
pcl5_item_cols = [f"pcl5_{i}" for i in range(1, 21)]
df_pcl5 = df_simplified[pcl5_item_cols]  # or new_df if not simplified

# Plot setup
n_items = len(pcl5_item_cols)
n_rows, n_cols = 4, 5  # 20 items: 4 rows x 5 columns grid

fig, axes = plt.subplots(n_rows, n_cols, figsize=(18, 10))
axes = axes.flatten()

# Histograms for each PCL-5 item
for idx, col in enumerate(pcl5_item_cols):
    axes[idx].hist(df_pcl5[col], bins=5, color='skyblue', edgecolor='black')  # responses 0–4
    axes[idx].set_title(col)
    axes[idx].set_xlabel("Response")
    axes[idx].set_ylabel("Subjects")

# Hide unused axes if any
for ax in axes[n_items:]:
    ax.set_visible(False)

plt.tight_layout()
plt.suptitle("Histograms of PCL-5 Item Responses", fontsize=16, y=1.02)
plt.show()

# %%
# Descriptive statistics
print(df_pcl5.describe())

# %%
# Cronbach's alpha for PCL-5
c_alpha_pcl5 = cronbach_alpha(df_pcl5)
print(f"Cronbach_alpha value: {c_alpha_pcl5:.4f}")

# %% [markdown]
# PCL‑5 items show that most people in the sample chose 0 (“Not at all”) on each PCL‑5 item, and fewer people chose higher scores from 1 to 4. The average score for every item is around 1, and at least half of the participants scored 0 on each question. The Cronbach’s alpha of 0.98 shows that the 20 items are extremely consistent with each other, so the PCL‑5 total score is a very reliable measure of PTSD symptoms in this dataset, supporting the use of the summed PCL‑5 total score. 

# %%
# Compute the total PCL-5 score
df_simplified['pcl5_total'] = df_simplified[pcl5_item_cols].sum(axis=1)

# Visualize histogram of PCL-5 total score
plt.figure(figsize=(7, 4))
plt.hist(df_simplified['pcl5_total'], bins=15, color='skyblue', edgecolor='black')
plt.xlabel("PCL-5 Total Score")
plt.ylabel("Subjects")
plt.title("Distribution of PCL-5 Total Score")
plt.show()

# Visualize boxplot of PCL-5 total score
plt.figure(figsize=(5, 3))
sns.boxplot(x=df_simplified['pcl5_total'], color='skyblue')
plt.xlabel("PCL-5 Total Score")
plt.title("Boxplot of PCL-5 Total Score")
plt.show()

# Summary statistics
print("PCL-5 total score summary statistics:")
print(df_simplified['pcl5_total'].describe())

# Shapiro-Wilk test for normality
stat, p_value = shapiro(df_simplified['pcl5_total'])
print(f"Shapiro-Wilk test statistic: {stat:.4f}")
print(f"P-value: {p_value:.4f}")

if p_value > 0.05:
    print("The PCL-5 total score distribution is compatible with normality.")
else:
    print("The PCL-5 total score distribution significantly deviates from normality.")

# Drop all PCL-5 item columns from the DataFrame
df_simplified = df_simplified.drop(columns=pcl5_item_cols)


# %% [markdown]
# The PCL‑5 total scores range from 0 to 80, with an average of about 19 points, so most people report relatively low to moderate PTSD symptoms. The histogram and boxplot show a right‑skewed pattern: many participants score in the low range, while a smaller group has very high scores, creating a long tail and many outliers. The Shapiro–Wilk test (statistic = 0.69, p < 0.0001) confirms that the PCL‑5 total score is far from normally distributed, so non‑parametric methods or the recommended clinical cut‑off (for example, ≥33 as “probable PTSD”) should be used instead of tests that assume normality.

# %% [markdown]
# Each of the 20 items is scored 0–4 and summed to a total score from 0 to 80.​
# 
# A total score of 33 or higher is commonly used as a provisional PTSD diagnosis cut-off (i.e., “likely PTSD” vs “below threshold”).

# %%
# PCL-5 categories based on total score (0–80)
pcl5_categories = [
    "Below cut-off" if score < 33 else
    "Probable PTSD"
    for score in df_simplified['pcl5_total']
]

pcl5_cat_series = pd.Series(pcl5_categories, name='PCL-5 Category')

plt.figure(figsize=(7, 5))
sns.countplot(
    x=pcl5_cat_series,
    palette=sns.color_palette("Blues", n_colors=2),
    edgecolor='black'
)
plt.xlabel('PCL-5 Category')
plt.ylabel('Subjects')
plt.title('Distribution of PCL-5 Clinical Categories')
plt.show()

print(pcl5_cat_series.value_counts())

# %% [markdown]
# #### MSPSS

# %%
# Select only MSPSS item columns
mspss_item_cols = [f"mspss_{i}" for i in range(1, 13)]
df_mspss = df_simplified[mspss_item_cols]  # or new_df if not simplified

# Plot setup
n_items = len(mspss_item_cols)
n_rows, n_cols = 3, 4  # 12 items: 3 rows x 4 columns grid

fig, axes = plt.subplots(n_rows, n_cols, figsize=(18, 8))
axes = axes.flatten()

# Histograms for each MSPSS item
for idx, col in enumerate(mspss_item_cols):
    axes[idx].hist(df_mspss[col], bins=7, color='skyblue', edgecolor='black')  # responses 1–7
    axes[idx].set_title(col)
    axes[idx].set_xlabel("Response")
    axes[idx].set_ylabel("Subjects")

# Hide unused axes if any
for ax in axes[n_items:]:
    ax.set_visible(False)

plt.tight_layout()
plt.suptitle("Histograms of MSPSS Item Responses", fontsize=16, y=1.02)
plt.show()


# %%
# Descriptive statistics
print(df_mspss.describe())

# %%
# Cronbach's alpha for MSPSS
c_alpha_pcl5 = cronbach_alpha(df_mspss)
print(f"Cronbach_alpha value: {c_alpha_pcl5:.4f}")

# %% [markdown]
# MSPSS item scores are generally in the mid–high range, with medians around 4–6 on the 0–7 scale, indicating moderate to high perceived social support in this sample. Variability is substantial (standard deviations about 2.4–2.7), showing that support levels differ meaningfully across participants. The Cronbach’s alpha of 0.88 indicates good internal consistency, so the MSPSS total (and subscale) scores can be considered reliable measures of perceived social support.

# %%
# Compute the total MSPSS score
mspss_item_cols = [f"mspss_{i}" for i in range(1, 13)]
df_simplified['mspss_total'] = df_simplified[mspss_item_cols].sum(axis=1)

# Visualize histogram of MSPSS total score
plt.figure(figsize=(7, 4))
plt.hist(df_simplified['mspss_total'], bins=15, color='skyblue', edgecolor='black')
plt.xlabel("MSPSS Total Score")
plt.ylabel("Subjects")
plt.title("Distribution of MSPSS Total Score")
plt.show()

# Visualize boxplot of MSPSS total score
plt.figure(figsize=(5, 3))
sns.boxplot(x=df_simplified['mspss_total'], color='skyblue')
plt.xlabel("MSPSS Total Score")
plt.title("Boxplot of MSPSS Total Score")
plt.show()

# Summary statistics
print("MSPSS total score summary statistics:")
print(df_simplified['mspss_total'].describe())

# Shapiro-Wilk test for normality
stat, p_value = shapiro(df_simplified['mspss_total'])
print(f"Shapiro-Wilk test statistic: {stat:.4f}")
print(f"P-value: {p_value:.4f}")

if p_value > 0.05:
    print("The MSPSS total score distribution is compatible with normality.")
else:
    print("The MSPSS total score distribution significantly deviates from normality.")

# Drop all MSPSS item columns from the DataFrame
df_simplified = df_simplified.drop(columns=mspss_item_cols)


# %% [markdown]
# MSPSS total scores range from 12 to 84, with a mean of about 53.5, indicating generally moderate to high perceived social support in this sample. The histogram and boxplot show a right‑skewed distribution, with many participants scoring in the higher support range and fewer at very low scores. The Shapiro–Wilk test (statistic = 0.94, p < 0.001) confirms that the MSPSS total score is not normally distributed, so non‑parametric tests or category-based analyses (low, moderate, high support) are more appropriate than parametric methods that assume normality.​

# %% [markdown]
# MSPSS does not have universal clinical cut-offs, but the original guidelines often use three bands based on the mean item score (total score ÷ 12):
# - Mean 1–2.9: Low support
# - Mean 3–5: Moderate support
# - Mean 5.1–7: High support

# %%
# Compute mean item score
mspss_mean = df_simplified['mspss_total'] / 12.0

# MSPSS categories based on mean item score
mspss_categories = [
    "Low support" if m < 3 else
    "Moderate support" if m <= 5 else
    "High support"
    for m in mspss_mean
]

mspss_cat_series = pd.Series(mspss_categories, name='MSPSS Category')

plt.figure(figsize=(7, 5))
sns.countplot(
    x=mspss_cat_series,
    palette=sns.color_palette("Blues", n_colors=3),
    edgecolor='black'
)
plt.xlabel('MSPSS Category')
plt.ylabel('Subjects')
plt.title('Distribution of MSPSS Support Categories')
plt.show()

print(mspss_cat_series.value_counts())


# %% [markdown]
# #### SWLS

# %%
# Select only SWLS item columns
swls_item_cols = [f"swls_{i}" for i in range(1, 6)]
df_swls = df_simplified[swls_item_cols]  # or new_df if not simplified

# Plot setup
n_items = len(swls_item_cols)
n_rows, n_cols = 1, 5  # 5 items: 1 row x 5 columns grid

fig, axes = plt.subplots(n_rows, n_cols, figsize=(18, 4))
axes = axes.flatten()

# Histograms for each SWLS item
for idx, col in enumerate(swls_item_cols):
    axes[idx].hist(df_swls[col], bins=7, color='skyblue', edgecolor='black')  # responses 1–7
    axes[idx].set_title(col)
    axes[idx].set_xlabel("Response")
    axes[idx].set_ylabel("Subjects")

plt.tight_layout()
plt.suptitle("Histograms of SWLS Item Responses", fontsize=16, y=1.05)
plt.show()


# %%
# Descriptive statistics
print(df_swls.describe())

# %%
# Cronbach's alpha for swls
c_alpha_swls = cronbach_alpha(df_swls)
print(f"Cronbach_alpha value: {c_alpha_pcl5:.4f}")

# %% [markdown]
# SWLS item scores are mostly in the mid range of the 0–7 scale, with medians of 4 on all items and means between about 3.7 and 4.2. Variation is moderate (standard deviations around 2.0–2.4), indicating meaningful differences in life satisfaction across participants. The Cronbach’s alpha of 0.88 shows good internal consistency, so the SWLS total score can be treated as a reliable measure of overall life satisfaction in this sample.

# %%
# Compute the total SWLS score
df_simplified['swls_total'] = df_simplified[swls_item_cols].sum(axis=1)

# Visualize histogram of SWLS total score
plt.figure(figsize=(7, 4))
plt.hist(df_simplified['swls_total'], bins=15, color='skyblue', edgecolor='black')
plt.xlabel("SWLS Total Score")
plt.ylabel("Subjects")
plt.title("Distribution of SWLS Total Score")
plt.show()

# Visualize boxplot of SWLS total score
plt.figure(figsize=(5, 3))
sns.boxplot(x=df_simplified['swls_total'], color='skyblue')
plt.xlabel("SWLS Total Score")
plt.title("Boxplot of SWLS Total Score")
plt.show()

# Summary statistics
print("SWLS total score summary statistics:")
print(df_simplified['swls_total'].describe())

# Shapiro-Wilk test for normality
stat, p_value = shapiro(df_simplified['swls_total'])
print(f"Shapiro-Wilk test statistic: {stat:.4f}")
print(f"P-value: {p_value:.4f}")

if p_value > 0.05:
    print("The SWLS total score distribution is compatible with normality.")
else:
    print("The SWLS total score distribution significantly deviates from normality.")

# Drop all SWLS item columns from the DataFrame
df_simplified = df_simplified.drop(columns=swls_item_cols)


# %% [markdown]
# SWLS total scores range from 5 to 35, with a mean of about 19.4 and an interquartile range from 13 to 25.3, indicating generally moderate levels of life satisfaction in this sample. The histogram and boxplot show a slightly skewed, non-normal distribution, which is confirmed by the Shapiro–Wilk test (statistic = 0.97, p < 0.001); therefore, non-parametric methods or categorical groupings are preferable for analyses involving the SWLS total score.​
# 
# For SWLS there are commonly used interpretive bands for the total score (5–35):
# - 5–9: Extremely dissatisfied
# - 10–14: Dissatisfied
# - 15–19: Slightly dissatisfied
# - 20–24: Slightly satisfied
# - 25–29: Satisfied
# - 30–35: Extremely satisfied

# %%
# SWLS categories based on total score (5–35)
swls_categories = [
    "Extremely dissatisfied" if s <= 9 else
    "Dissatisfied" if s <= 14 else
    "Slightly dissatisfied" if s <= 19 else
    "Slightly satisfied" if s <= 24 else
    "Satisfied" if s <= 29 else
    "Extremely satisfied"
    for s in df_simplified['swls_total']
]

swls_cat_series = pd.Series(swls_categories, name='SWLS Category')

plt.figure(figsize=(10, 5))
sns.countplot(
    x=swls_cat_series,
    palette=sns.color_palette("Blues", n_colors=6),
    edgecolor='black'
)
plt.xlabel('SWLS Category')
plt.ylabel('Subjects')
plt.title('Distribution of SWLS Satisfaction Categories')
plt.xticks(rotation=30, ha='right')
plt.show()

print(swls_cat_series.value_counts())


# %% [markdown]
# #### WHO5

# %%
# Select only WHO-5 item columns
who5_item_cols = [f"who5_{i}" for i in range(1, 6)]
df_who5 = df_simplified[who5_item_cols]  # or new_df if not simplified

# Plot setup
n_items = len(who5_item_cols)
n_rows, n_cols = 1, 5  # 5 items: 1 row x 5 columns

fig, axes = plt.subplots(n_rows, n_cols, figsize=(18, 4))
axes = axes.flatten()

# Histograms for each WHO-5 item
for idx, col in enumerate(who5_item_cols):
    axes[idx].hist(df_who5[col], bins=6, color='skyblue', edgecolor='black')  # responses 0–5
    axes[idx].set_title(col)
    axes[idx].set_xlabel("Response")
    axes[idx].set_ylabel("Subjects")

plt.tight_layout()
plt.suptitle("Histograms of WHO-5 Item Responses", fontsize=16, y=1.05)
plt.show()

# Descriptive statistics
print(df_who5.describe())

# Cronbach's alpha for WHO-5
c_alpha_who5 = cronbach_alpha(df_who5)
print(f"Cronbach_alpha value: {c_alpha_who5:.4f}")

# %% [markdown]
# WHO‑5 item scores cluster around the middle of the 0–5 scale, with medians of 2–3 and means between about 2.6 and 3.0. This indicates moderate levels of positive well‑being, with some participants reporting very low or very high scores. The Cronbach’s alpha of 0.62 reflects moderate internal consistency, which is lower than typical WHO‑5 reports but still suggests that the items measure a common construct, so the total score can be used cautiously as an index of well‑being.​

# %%
# Compute the total WHO-5 score
df_simplified['who5_total'] = df_simplified[who5_item_cols].sum(axis=1)

# Visualize histogram of WHO-5 total score
plt.figure(figsize=(7, 4))
plt.hist(df_simplified['who5_total'], bins=15, color='skyblue', edgecolor='black')
plt.xlabel("WHO-5 Total Score")
plt.ylabel("Subjects")
plt.title("Distribution of WHO-5 Total Score")
plt.show()

# Visualize boxplot of WHO-5 total score
plt.figure(figsize=(5, 3))
sns.boxplot(x=df_simplified['who5_total'], color='skyblue')
plt.xlabel("WHO-5 Total Score")
plt.title("Boxplot of WHO-5 Total Score")
plt.show()

# Summary statistics
print("WHO-5 total score summary statistics:")
print(df_simplified['who5_total'].describe())

# Shapiro-Wilk test for normality
stat, p_value = shapiro(df_simplified['who5_total'])
print(f"Shapiro-Wilk test statistic: {stat:.4f}")
print(f"P-value: {p_value:.4f}")

if p_value > 0.05:
    print("The WHO-5 total score distribution is compatible with normality.")
else:
    print("The WHO-5 total score distribution significantly deviates from normality.")

# Drop all WHO-5 item columns from the DataFrame
df_simplified = df_simplified.drop(columns=who5_item_cols)


# %% [markdown]
# WHO‑5 total scores in your sample range from 6 to 23, with a mean of about 13.9 and an interquartile range from 10 to 18, indicating overall moderate well‑being. The histogram and boxplot show a slightly skewed, non‑normal distribution, which the Shapiro–Wilk test confirms (statistic = 0.95, p < 0.001). Because the distribution significantly deviates from normality, non‑parametric methods and the recommended WHO‑5 cut‑off (<13 vs ≥13) are more appropriate than parametric tests that assume normality.
# 
# For the WHO‑5, standard interpretation uses the total raw score (0–25) or the percentage score (0–100) rather than multiple qualitative bands. The core cut-off from the WHO‑5 manual is:
# - Raw total < 13 (or percentage < 52): “Possible depression / poor well‑being”, follow‑up recommended.
# - Raw total ≥ 13 (percentage ≥ 52): “No indication of depression / acceptable well‑being.”

# %%
# WHO-5 categories based on total score (0–25)
who5_categories = [
    "Possible depression" if s < 13 else
    "No indication of depression"
    for s in df_simplified['who5_total']
]

who5_cat_series = pd.Series(who5_categories, name='WHO-5 Category')

plt.figure(figsize=(8, 5))
sns.countplot(
    x=who5_cat_series,
    palette=sns.color_palette("Blues", n_colors=2),
    edgecolor='black'
)
plt.xlabel('WHO-5 Category')
plt.ylabel('Subjects')
plt.title('Distribution of WHO-5 Well-being Categories')
plt.show()

print(who5_cat_series.value_counts())


# %%
df_simplified

# %% [markdown]
# The current working dataset now contains 200 participants and 16 variables: age, gender, education, marital status, income, their corresponding label columns, and the total scores for all psychometric scales (IAT, AUDIT, DAST‑10, PGSI, PCL‑5, MSPSS, SWLS, WHO‑5). This structure is appropriate for moving on to bivariate and multivariate analyses, since all item-level columns have been collapsed into reliable total scores and the key sociodemographic variables are preserved in both numeric and labeled form.

# %% [markdown]
# # Bivariate EDA
# - Correlations between continuous scales (Spearman if non‑normal) and between each scale and age/income.
# - Boxplots or violin plots of key scores by gender, income groups, or other relevant categories.

# %% [markdown]
# ## Spearman correlation test

# %% [markdown]
# The Spearman rank-order correlation coefficient is a nonparametric measure of the monotonicity of the relationship between two datasets. Like other correlation coefficients, this one varies between -1 and +1 with 0 implying no correlation. Correlations of -1 or +1 imply an exact monotonic relationship. Positive correlations imply that as x increases, so does y. Negative correlations imply that as x increases, y decreases.
# 
# The p-value roughly indicates the probability of an uncorrelated system producing datasets that have a Spearman correlation at least as extreme as the one computed from these datasets. Although calculation of the p-value does not make strong assumptions about the distributions underlying the samples, it is only accurate for very large samples (>500 observations).
# https://docs.scipy.org/doc/scipy/reference/generated/scipy.stats.spearmanr.html 
# https://pandas.pydata.org/docs/reference/api/pandas.DataFrame.corr.html

# %%
target = "iat_total"
features = [
    "pcl5_total", "audit_total", "dast_total", "pgsi_total",
    "who5_total", "swls_total", "age", "income"
]

rows = []
for var in features:
    valid = df_simplified[[target, var]]
    rho, p = spearmanr(valid[target], valid[var])
    rows.append({"var": var, "rho": rho, "p": p, "n": len(valid)})

spearman_iat = pd.DataFrame(rows).sort_values("rho", ascending=False)
print(spearman_iat)

plt.figure(figsize=(7, 5))
sns.barplot(
    data=spearman_iat,
    x="rho", y="var",
    palette="coolwarm",
    hue="rho", dodge=False, legend=False
)
plt.axvline(0, color="black", linewidth=1)
plt.xlabel("Spearman ρ with iat_total")
plt.ylabel("Variable")
plt.title("Spearman Correlations: iat_total vs Features")
plt.tight_layout()
plt.show()

# %% [markdown]
# Strongest links: **audit_total (ρ ≈ −0.66) and income (ρ ≈ −0.58)**: higher IAT with higher alcohol problems and lower income (assuming higher code = higher income).
# 
# Moderate: **swls_total and who5_total (ρ ≈ −0.28)**: higher IAT with lower life satisfaction and well‑being.
# 
# Weak/none: pcl5_total small positive; pgsi_total, dast_total, age near zero.

# %%
n_features = len(features)
n_cols = 4
n_rows = (n_features + n_cols - 1) // n_cols  # ceil

fig, axes = plt.subplots(n_rows, n_cols, figsize=(4*n_cols, 3.5*n_rows))
axes = axes.flatten()  # make it 1D for easy indexing

for i, var in enumerate(features):
    ax = axes[i]
    sns.scatterplot(
        data=df_simplified,
        x=var,
        y=target,
        ax=ax,
        alpha=0.7
    )
    ax.set_title(f"{target} vs {var}")
    ax.set_xlabel(var)
    ax.set_ylabel(target)

# Hide any unused subplots
for j in range(i+1, len(axes)):
    fig.delaxes(axes[j])

plt.tight_layout()
plt.show()


# %% [markdown]
# - audit_total and income: clear negative monotonic trends → matches strong negative ρ.
# - who5_total and swls_total: visible downward pattern → consistent with moderate negative ρ.
# - pcl5_total: weak upward tendency → small positive ρ.
# - dast_total, pgsi_total, age: mostly clouds with no clear slope → ρ near 0 makes sense.

# %% [markdown]
# ## Kruskal-Wallis test

# %% [markdown]
# In contrast, the **Kruskal-Wallis test** was used to compare IAT scores across categorical groups (e.g., gender categories). It is a nonparametric alternative to one-way ANOVA for assessing whether the medians differ between three or more independent groups when data are not normally distributed.
# 
# The Kruskal-Wallis test is essentially **an extension of the Mann-Whitney U test**, which is used for comparing exactly two independent groups on a continuous or ordinal variable under non-normality assumptions. Therefore, the Mann-Whitney U test is applied when comparing two groups, while Kruskal-Wallis is appropriate for three or more groups.
# 
# Thus, the choice of test depends on whether the aim is to assess association between variables (Spearman), or differences between two groups (Mann-Whitney U), or differences among three or more groups (Kruskal-Wallis) under non-normal data conditions.
# 
# https://www.slideshare.net/slideshow/kruskal-wallis-test-friedman-test-spearman-correlation/197229012

# %% [markdown]
# Kruskal–Wallis ranks all 200 IAT scores from lowest to highest, then checks if the average rank differs across gender groups.
# H is the test statistic measuring how different the average ranks are.
# p < 0.05 tells that at least one group has a different rank pattern than the others. 
# ε² tells you how much a category explains IAT variation in %.
# Kruskal–Wallis only says "at least one group differs," not which ones.
# For this reason, post-hoc tests checked all possible pairs to test which ones are significant.

# %%
import itertools
from scipy.stats import kruskal, mannwhitneyu

def holm_bonferroni(pvals):
    m = len(pvals)
    order = np.argsort(pvals)
    p_sorted = np.array(pvals)[order]
    adj = np.empty(m, dtype=float)
    running_max = 0.0
    for i, p in enumerate(p_sorted):
        val = (m - i) * p
        running_max = max(running_max, val)
        adj[i] = min(1.0, running_max)
    p_adj = np.empty(m, dtype=float)
    p_adj[order] = adj
    return p_adj

def rank_biserial_from_U(U, n1, n2):
    return 1 - (2 * U) / (n1 * n2)

def hodges_lehmann(a, b):
    return float(np.median(np.subtract.outer(b, a).ravel()))

def kruskal_by_group(df, score_col, group_col):
    # Drop missing
    df_ = df[[score_col, group_col]]

    # Descriptives
    desc = df_.groupby(group_col)[score_col].agg(
        count="count", median="median", mean="mean", std="std"
    )
    print(f"\n=== {score_col} by {group_col} ===")
    print(desc.round(2))

    groups = [g[score_col].values for _, g in df_.groupby(group_col)]
    labels = df_[group_col].unique().tolist()
    k = len(groups)

    if k < 2:
        print("\nNot enough groups for a test.")
        return

    if k == 2:
        # Mann–Whitney case
        A, B = groups
        la, lb = labels
        U, p = mannwhitneyu(A, B, alternative="two-sided", method="auto")
        r_rb = rank_biserial_from_U(U, len(A), len(B))
        print(f"\n--- Mann–Whitney U ({la} vs {lb}) ---")
        print(f"U = {U:.2f}, p = {p:.4f}, rank-biserial r = {r_rb:.3f}")
        return

    # Kruskal–Wallis
    H, p_kw = kruskal(*groups)
    n_tot = sum(len(g) for g in groups)
    eps2 = (H - k + 1) / (n_tot - k) if (n_tot - k) > 0 else np.nan

    print(f"\n--- Kruskal–Wallis (k={k} groups) ---")
    print(f"H = {H:.3f}, df = {k - 1}, p = {p_kw:.4f}")
    print(f"Effect size: epsilon-squared ε² = {eps2:.3f}")

    # Post-hoc pairwise Mann–Whitney + Holm
    pairs = []
    for gA, gB in itertools.combinations(desc.index, 2):
        A = df_.loc[df_[group_col] == gA, score_col].values
        B = df_.loc[df_[group_col] == gB, score_col].values
        U, p_raw = mannwhitneyu(A, B, alternative="two-sided", method="auto")
        r_rb = rank_biserial_from_U(U, len(A), len(B))
        hl = hodges_lehmann(A, B)
        pairs.append({
            "Group_A": gA, "Group_B": gB,
            "n_A": len(A), "n_B": len(B),
            "U": float(U), "p_raw": float(p_raw),
            "r_rank_biserial": float(r_rb),
            "HL_diff(B-A)": float(hl)
        })

    pairs_df = pd.DataFrame(pairs)
    pairs_df["p_holm"] = holm_bonferroni(pairs_df["p_raw"].values)
    pairs_df = pairs_df.sort_values("p_holm").reset_index(drop=True)

    print("\nPost-hoc pairwise Mann–Whitney (Holm-corrected):")
    print(pairs_df.round(4).to_string(index=False))

    sig = pairs_df[pairs_df["p_holm"] < 0.05]
    if not sig.empty:
        print("\nSignificant pairs (p < 0.05 after Holm):")
        for _, r in sig.iterrows():
            direction = "B > A" if r["r_rank_biserial"] > 0 else "A > B"
            print(f" - {r['Group_A']} vs {r['Group_B']}: {direction}, "
                  f"HL = {r['HL_diff(B-A)']:.2f}, p = {r['p_holm']:.4f}")
    else:
        print("\nNo significant pairs after Holm correction.")



# %%
kruskal_by_group(df_simplified, "iat_total", "gender_label")

# %% [markdown]
# IAT scores differed significantly by gender (Kruskal–Wallis H(3) = 9.72, p = .021, ε²=0.03), with post-hoc tests showing that female participants reported higher scores than non-binary participants (adjusted p = .030).
# Only Female vs Non-binary remained significant, all other pairs (Female vs Male, Male vs Non-binary, etc.) were not significant after correction.

# %%
kruskal_by_group(df_simplified, "iat_total", "school_labels")

# %% [markdown]
# IAT scores differed strongly by education (Kruskal–Wallis H(5) = 60.99, p < .001, ε² = .29, large effect). Participants with Middle and High education reported much higher IAT scores than those with Master or Doctoral degrees, with median differences often exceeding 40–50 points.

# %%
kruskal_by_group(df_simplified, "iat_total", "marital_labels")

# %% [markdown]
# IAT scores differed strongly by marital status (Kruskal–Wallis H(5) = 54.71, p < .001, ε² = .26, large effect). Single participants reported much higher IAT scores (median = 82) than all other marital groups, with median differences ranging from 24 to 62 points.

# %%
fig, axes = plt.subplots(1, 3, figsize=(15, 4), sharey=True)

# 1. IAT by gender
sns.boxplot(x="gender_label", y="iat_total",
            data=df_simplified, ax=axes[0])
axes[0].set_title("IAT by Gender")
axes[0].set_xlabel("Gender")
axes[0].set_ylabel("IAT total")

# 2. IAT by education
sns.boxplot(x="school_labels", y="iat_total",
            data=df_simplified, ax=axes[1])
axes[1].set_title("IAT by Education")
axes[1].set_xlabel("Education")
axes[1].set_ylabel("")

# 3. IAT by marital status
sns.boxplot(x="marital_labels", y="iat_total",
            data=df_simplified, ax=axes[2])
axes[2].set_title("IAT by Marital Status")
axes[2].set_xlabel("Marital status")
axes[2].set_ylabel("")

plt.tight_layout()
plt.show()

# %% [markdown]
# - Gender: Females and “Prefer not to say” show higher medians and upper‑quartile IAT scores than males and non‑binary participants, matching the significant H and p you found.
# - Education: Very strong gradient; “Middle” and “High” education groups cluster at much higher IAT, while “Master” and “Doctoral” are clearly lower, with compressed boxes near the bottom range.
# - Marital status: Singles stand out with a much higher median and wide spread, while separated and widowed groups show lower medians; married and divorced sit in the middle, exactly as your numeric summary indicated.
# 
# Education and marital status as main sociodemographic axes for personas. 
# 
# Gender as a secondary/background descriptor.

# %% [markdown]
# # Multivariate EDA
# A correlation heatmap for all composite scores to show how dimensions (addictions, PTSD, well‑being, social support) relate.​
# Multivariate EDA includes also dimensionality reduction (PCA, FAMD) to reduce many correlated features to 2-4 interpretable components. 

# %%
corr = df_simplified[[
    "iat_total","audit_total","dast_total","pgsi_total",
    "pcl5_total","mspss_total","swls_total","who5_total",
    "age","income"
]].corr(method="spearman")

sns.heatmap(corr, annot=True, fmt=".2f", cmap="coolwarm",
            vmin=-1, vmax=1, center=0)
plt.title("Spearman Correlation Heatmap (IAT and other scores)")
plt.tight_layout()
plt.show()


# %% [markdown]
# This figure nicely motivates which variables to use as main axes for personas (IAT + alcohol + income + well‑being + life satisfaction, with weaker roles for age, drugs, gambling).

# %% [markdown]
# ## Data Standardization 

# %%

X = [[ 1., -2.,  2.],
     [ -2.,  1.,  3.],
     [ 4.,  1., -2.]]
transformer = RobustScaler().fit(X)
transformer
transformer.transform(X)

# %% [markdown]
# ## ONE-HOT ENCODING + PCA

# %%
features_num = [
    "iat_total", "audit_total", "who5_total",
    "swls_total", "pcl5_total", "mspss_total",
    "income", "age"
]

features_cat = ["gender_label", "school_labels", "marital_labels"]

df_mix = df_simplified[features_num + features_cat].dropna()


# %%
# One-hot encode categoricals
df_dummies = pd.get_dummies(df_mix[features_cat], drop_first=False)

# Standardize numeric
scaler = StandardScaler()
X_num_scaled = scaler.fit_transform(df_mix[features_num])

# Combine
X_pca_input = np.hstack([X_num_scaled, df_dummies.values])

# PCA with 3 components
pca = PCA(n_components=3, random_state=42)
X_pca = pca.fit_transform(X_pca_input)
print("PCA explained variance ratio:", pca.explained_variance_ratio_)

# Attach PCs to dataframe
df_pca_comp = df_mix.copy()
df_pca_comp[["PC1_pca", "PC2_pca", "PC3_pca"]] = X_pca

# Create IAT levels
bins = [0, 39, 69, 100]
labels = ["Average", "Problematic", "Severe"]
df_pca_comp["iat_level"] = pd.cut(df_pca_comp["iat_total"], bins=bins, labels=labels)

# %% [markdown]
# ## FAMD (leaving mixed data)

# %%
famd = prince.FAMD(
    n_components=3,
    n_iter=10,
    copy=True,
    check_input=True,
    engine="sklearn",    
    random_state=42
)

famd = famd.fit(df_mix)
X_famd = famd.transform(df_mix)

df_famd_comp = df_mix.copy()
df_famd_comp[["PC1_famd", "PC2_famd", "PC3_famd"]] = X_famd.values
df_famd_comp["iat_level"] = pd.cut(df_famd_comp["iat_total"], bins=bins, labels=labels)


# %% [markdown]
# ## Comparison before clustering

# %%
plt.figure(figsize=(7, 5))
sns.scatterplot(
    data=df_pca_comp,
    x="PC1_pca", y="PC2_pca",
    hue="iat_level", palette="viridis", alpha=0.7, s=70
)
plt.xlabel(f"PC1 ({pca.explained_variance_ratio_[0]*100:.1f}%)")
plt.ylabel(f"PC2 ({pca.explained_variance_ratio_[1]*100:.1f}%)")
plt.title("PCA: PC1 vs PC2 colored by IAT level")
plt.legend(title="IAT level")
plt.grid(alpha=0.3)
plt.tight_layout()
plt.show()


# %%
plt.figure(figsize=(7, 5))
sns.scatterplot(
    data=df_famd_comp,
    x="PC1_famd", y="PC2_famd",
    hue="iat_level", palette="viridis", alpha=0.7, s=70
)
plt.xlabel("PC1 (FAMD)")
plt.ylabel("PC2 (FAMD)")
plt.title("FAMD: PC1 vs PC2 colored by IAT level")
plt.legend(title="IAT level")
plt.grid(alpha=0.3)
plt.tight_layout()
plt.show()


# %% [markdown]
# PCA plot:
# - IAT levels form very clear, compact groups.
# - “Average”, “Problematic”, “Severe” are almost linearly separated.
# Great structure for clustering/personas.
# 
# FAMD plot:
# - IAT levels are much more mixed.
# - PC1/PC2 seem driven more by categorical variables; IAT is not as clearly separated.

# %% [markdown]
# # Clustering 
# Kmeans
# https://www.datanovia.com/en/lessons/determining-the-optimal-number-of-clusters-3-must-know-methods/

# %%
X_pca_cluster = df_pca_comp[["PC1_pca", "PC2_pca", "PC3_pca"]].values

# %%
from sklearn.cluster import KMeans
import numpy as np
import pandas as pd

def optimalK(data, maxClusters):
    """
    Calculates KMeans optimal K using Gap Statistic from Tibshirani, Walther, Hastie
    Params:
        data: ndarray of shape (n_samples, n_features)
        maxClusters: Maximum number of clusters to test for
    Returns: (optimal_k, resultsdf)
    """
    nrefs = 3
    gaps = np.zeros((len(range(1, maxClusters)),))
    resultsdf = pd.DataFrame({'clusterCount': [], 'gap': []})
    
    for gap_index, k in enumerate(range(1, maxClusters)):
        refDisps = np.zeros(nrefs)

        # Generate reference datasets
        for i in range(nrefs):
            randomReference = np.random.random_sample(size=data.shape)
            km = KMeans(n_clusters=k, n_init=10, random_state=42)
            km.fit(randomReference)
            refDisps[i] = km.inertia_

        # Fit original data
        km = KMeans(n_clusters=k, n_init=10, random_state=42)
        km.fit(data)
        origDisp = km.inertia_

        # Calculate gap statistic
        gap = np.log(np.mean(refDisps)) - np.log(origDisp)
        gaps[gap_index] = gap

        # FIXED: Use pd.concat instead of .append()
        resultsdf = pd.concat([
            resultsdf,
            pd.DataFrame({'clusterCount': [k], 'gap': [gap]})
        ], ignore_index=True)

    optimal_k = gaps.argmax() + 1
    return optimal_k, resultsdf


# %%
X_pca_cluster = df_pca_comp[["PC1_pca", "PC2_pca", "PC3_pca"]].values

k_gap, gapdf = optimalK(X_pca_cluster, maxClusters=15)
print("Optimal k (Gap):", k_gap)

# Plot Gap statistic
plt.plot(gapdf.clusterCount, gapdf.gap, linewidth=3)
plt.scatter(
    gapdf[gapdf.clusterCount == k_gap].clusterCount,
    gapdf[gapdf.clusterCount == k_gap].gap,
    s=250, c='r'
)
plt.grid(True)
plt.xlabel('Cluster Count')
plt.ylabel('Gap Value')
plt.title('Gap Values by Cluster Count (PCA)')
plt.show()


# %%
from sklearn.cluster import KMeans
import matplotlib.pyplot as plt

max_cluster = 10
clusters = range(1, max_cluster)
wcss = []

for k in clusters:
    km = KMeans(
        n_clusters=k,
        init="k-means++",
        n_init=10,
        max_iter=300,
        random_state=42
    )
    km.fit(X_pca_cluster)
    wcss.append(km.inertia_)

plt.figure()
plt.plot(clusters, wcss, marker='o')
plt.title("Elbow Method (PCA space)")
plt.xlabel("Number of clusters (k)")
plt.ylabel("WCSS (Inertia)")
plt.grid(True)
plt.show()


# %%
from sklearn.cluster import KMeans
from sklearn.metrics import silhouette_score, silhouette_samples

# 1) Use PCA components as data
X = X_pca_cluster   # shape (n_samples, 3)

# 2) Choose k you want to inspect (e.g. 4)
n_clusters = 4
km = KMeans(n_clusters=n_clusters, init="k-means++",
            n_init=10, max_iter=300, random_state=42)
cluster_labels = km.fit_predict(X)

# 3) Average silhouette
silhouette_avg = silhouette_score(X, cluster_labels)
print("Average silhouette for k =", n_clusters, ":", silhouette_avg)

# 4) Silhouette for each point
each_silhouette_score = silhouette_samples(X, cluster_labels, metric="euclidean")

# 5) Plot
colorlist = ['b', 'g', 'r', 'c', 'm', 'y', 'k', 'orange']  # >= n_clusters
fig, ax = plt.subplots(figsize=(8, 6))
y_lower = 10

for i in range(n_clusters):
    ith_vals = each_silhouette_score[cluster_labels == i]
    ith_vals.sort()
    size_i = ith_vals.shape[0]
    y_upper = y_lower + size_i

    color = colorlist[i]
    ax.fill_betweenx(np.arange(y_lower, y_upper),
                     0, ith_vals,
                     facecolor=color, edgecolor=color, alpha=0.3)
    ax.text(-0.05, y_lower + 0.5*size_i, str(i))
    y_lower = y_upper + 10

ax.set_title(f"Silhouette plot (k = {n_clusters})")
ax.set_xlabel("Silhouette score")
ax.set_ylabel("Cluster label")
ax.axvline(x=silhouette_avg, color="red", linestyle="--")
ax.set_yticks([])
ax.set_xticks([-0.2, 0, 0.2, 0.4, 0.6, 0.8, 1])
plt.tight_layout()
plt.show()

# %%
# Final k-means with k=4
kmeans_final = KMeans(n_clusters=4, init="k-means++", 
                      n_init=10, max_iter=300, random_state=42)
df_pca_comp["cluster_final"] = kmeans_final.fit_predict(X_pca_cluster)


# %%
from scipy.cluster.hierarchy import dendrogram, linkage, fcluster
from sklearn.metrics import silhouette_score

# 1) Use your PCA components
X = X_pca_cluster  # shape (n_samples, 3)

# 2) Compute linkage matrix (agglomerative hierarchical clustering)
# Common linkage methods: 'ward', 'complete', 'average', 'single'
linkage_matrix = linkage(X, method='ward')  # Ward minimizes within-cluster variance

# 3) Plot dendrogram
plt.figure(figsize=(12, 7))
dendrogram(linkage_matrix)
plt.title('Hierarchical Clustering Dendrogram (Ward linkage)')
plt.xlabel('Sample index')
plt.ylabel('Distance')
plt.axhline(y=10, color='r', linestyle='--', label='Cut at k=4')  # adjust y for desired k
plt.legend()
plt.tight_layout()
plt.show()


# %%
# Option 1: Cut at specific height
# clusters = fcluster(linkage_matrix, t=10, criterion='distance')

# Option 2: Cut to get exactly k clusters (easier)
k = 4
clusters_hc = fcluster(linkage_matrix, t=k, criterion='maxclust')

# Add to dataframe
df_pca_comp['cluster_hc'] = clusters_hc

print("Hierarchical cluster sizes:")
print(df_pca_comp['cluster_hc'].value_counts().sort_index())


# %%
# Silhouette for hierarchical
sil_hc = silhouette_score(X, clusters_hc)
print(f"Silhouette (hierarchical, k={k}):", sil_hc)

# Compare cluster profiles
print("\n=== Hierarchical Clustering (k=4) ===")
print(df_pca_comp.groupby("cluster_hc")[[
    "iat_total", "audit_total", "who5_total", "swls_total"
]].mean().round(1))

print("\n=== K-means Clustering (k=4) ===")
print(df_pca_comp.groupby("cluster_final")[[
    "iat_total", "audit_total", "who5_total", "swls_total"
]].mean().round(1))


# %%
plt.figure(figsize=(8, 6))
sns.scatterplot(
    data=df_pca_comp,
    x="PC1_pca",
    y="PC2_pca",
    hue="cluster_hc",
    palette="Set2",
    alpha=0.7,
    s=70
)
plt.xlabel(f"PC1 ({pca.explained_variance_ratio_[0]*100:.1f}%)")
plt.ylabel(f"PC2 ({pca.explained_variance_ratio_[1]*100:.1f}%)")
plt.title("Hierarchical Clustering (Ward, k=4) in PCA space")
plt.legend(title="Cluster")
plt.grid(alpha=0.3)
plt.tight_layout()
plt.show()


# %% [markdown]
# plt.figure(figsize=(8, 6))
# sns.scatterplot(
#     data=df_pca_comp,
#     x="PC1_pca",
#     y="PC2_pca",
#     hue="cluster_hc",
#     palette="Set2",
#     alpha=0.7,
#     s=70
# )
# plt.xlabel(f"PC1 ({pca.explained_variance_ratio_[0]*100:.1f}%)")
# plt.ylabel(f"PC2 ({pca.explained_variance_ratio_[1]*100:.1f}%)")
# plt.title("Hierarchical Clustering (Ward, k=4) in PCA space")
# plt.legend(title="Cluster")
# plt.grid(alpha=0.3)
# plt.tight_layout()
# plt.show()
# 


