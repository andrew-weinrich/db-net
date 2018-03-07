#!/bin/bash

# very simple api test script

# starts the service, posts data, retrieves it in a sorted order, normalizes the JSON, then terminates the service
post_data() {
    DELIMITER="$1"
    TEST_ID="$2"
    
    #python service.py 2>&1 &
    python service.py > /dev/null 2>&1 &
    SERVICE_PID=$!
    sleep 3
    
    while read line; do
        curl -s -X POST -d "$line" "http://localhost:8080/records?delimiter=$DELIMITER"
    done < "input${TEST_ID}.txt"
    
    curl -s -X GET "http://localhost:8080/records/${SORT_TYPE}" > "test_output-${TEST_ID}-${SORT_TYPE}-unsorted.txt"
    
    # rearrange the data into key-ordered format
    python parse_json.py < "test_output-${TEST_ID}-${SORT_TYPE}-unsorted.txt" > "test_output-${TEST_ID}-${SORT_TYPE}.txt"
    
    kill $SERVICE_PID
    wait $SERVICE_PID > /dev/null 2>&1
}



SORT_TYPES=(gender birthdate name)

# different delimiters used, with a placeholder at index 0
DELIMITERS=('zero_placeholder' '%20%7C%20' '%2C%20' '%20')
DELIMITER_COUNT=`expr ${#DELIMITERS[@]} - 1`

for SORT_TYPE in "${SORT_TYPES[@]}"; do
    for TEST_ID in `seq 1 $DELIMITER_COUNT`; do
        post_data ${DELIMITERS[$TEST_ID]} $TEST_ID
        
        echo "Test ${TEST_ID} $SORT_TYPE:"
        diff "test_output-${TEST_ID}-${SORT_TYPE}.txt" "output-${SORT_TYPE}-api.txt" 

        rm "test_output-${TEST_ID}-${SORT_TYPE}.txt"
        rm "test_output-${TEST_ID}-${SORT_TYPE}-unsorted.txt"
    done

done