#!/bin/bash

# very simple api test script

# starts the service, posts data, retrieves it in a sorted order, normalizes the JSON, then terminates the service
post_data() {
    TEST_ID="$1"
    
    APPLICATION="RecordAPI/bin/Debug/netcoreapp2.0/RecordAPI.dll"
    HOST="localhost:5000"
    
    dotnet $APPLICATION > /dev/null 2>&1 &
    SERVICE_PID=$!
    sleep 3
    
    while read line; do
        curl -s -X POST -d "$line" "http://$HOST/api/records"
    done < "input${TEST_ID}.txt"
    
    curl -s -X GET "http://$HOST/api/records/${SORT_TYPE}" > "test_output-${TEST_ID}-${SORT_TYPE}-unsorted.txt"
    
    # rearrange the data into key-ordered format
    python parse_json.py < "test_output-${TEST_ID}-${SORT_TYPE}-unsorted.txt" > "test_output-${TEST_ID}-${SORT_TYPE}.txt"
    
    kill $SERVICE_PID
    wait $SERVICE_PID > /dev/null 2>&1
}



SORT_TYPES=(gender birthdate name)

DELIMITER_COUNT=3

for SORT_TYPE in "${SORT_TYPES[@]}"; do
    for TEST_ID in `seq 1 $DELIMITER_COUNT`; do
        post_data $TEST_ID
        
        echo "Test ${TEST_ID} $SORT_TYPE:"
        diff "test_output-${TEST_ID}-${SORT_TYPE}.txt" "output-${SORT_TYPE}-api.txt" 

        rm "test_output-${TEST_ID}-${SORT_TYPE}.txt"
        rm "test_output-${TEST_ID}-${SORT_TYPE}-unsorted.txt"
    done

done