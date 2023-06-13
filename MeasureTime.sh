#!/bin/bash

project_path="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
unity_executable="/Applications/Unity/Hub/Editor/2021.3.25f1/Unity.app/Contents/MacOS/Unity"
method_name="JD.EditorTimeMeasurements.LogEditorStartupTime"
reports_folder="${project_path}/Reports"
time_file="${reports_folder}/startup-times.txt"

execute_unity_and_measure_time() {
    local task_name="$1"
    local log_file="${reports_folder}/${task_name}.log"

    start_time=$(date +%s.%N)
    "${unity_executable}" -projectPath "${project_path}" -executeMethod "${method_name}" -logFile "${log_file}" -quit
    end_time=$(date +%s.%N)

    execution_time=$(echo "${end_time} - ${start_time}" | bc)
    echo "Startup time ${task_name}: ${execution_time} seconds." >> "${time_file}"
}

# Create the Reports folder if it doesn't exist
if [ ! -d "${reports_folder}" ]; then
    mkdir "${reports_folder}"
fi

# First run: Delete Library folder first
# Delete Unity's Library folder
rm -rf "${project_path}/Library"
execute_unity_and_measure_time "fresh_start"

# Second run - Check how long it takes to open the project with the library folder already existing
execute_unity_and_measure_time "subsequent_start"