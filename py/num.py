4   # int
2.5 # float
0.  # ...
1j  # complex

1_000 # digit grouping (python v3.6+)

1+2*3/4 # arithmetic: 2.5
9 % 2   # remainder: 1
4 ** 2  # squared: 16
4 ** 3  # cubed: 64

round(12.3456, 2) # 12.35

import math
float('inf')  == math.inf  # True
float('-inf') == -math.inf # True

# nan
float('nan')
math.nan
float('nan') == math.nan # False

# check for nan
n = float('nan')
math.isnan(n) # True
n != n        # True

# check for whole number
2.1 % 1 == 0 # False
2.0 % 1 == 0 # Tru

(2.2).is_integer() # False
(2.0).is_integer() # True

2.3 == int(2.3) # False
2.0 == int(2.0) # True