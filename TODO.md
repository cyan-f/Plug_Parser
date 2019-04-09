# Scuffed Elements

* Thread safety not assured.
* Unclear when vibe values are 0 - 100 or 0.0 - 1.0.
    - Strength: 0 - 100
    - Power: 0.0 - 1.0
    - UPDATE: Refactoring everything to be "Strength". Conversion to "Power" will only occur at vibration step.
* Catch toy disconnections.

# Refactor

* Add method headers
* Remove regions outside of Plug_Parser.cs