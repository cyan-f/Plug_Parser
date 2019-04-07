# Scuffed Elements

* Thread safety not assured.
* UI elements are updated unsafely.
* Unclear when vibe values are 0 - 100 or 0.0 - 1.0.
    - Strength: 0 - 100
    - Power: 0.0 - 1.0
    - UPDATE: Refactoring everything to be "Strength". Conversion to "Power" will only occur at vibration step.
* Catch toy disconnections.
* Desperately need to organize classes, and add method headers.
    - Especially in Device_Manager and Power_Calculator.