#!/usr/bin/python


import json
import sys


# parses input JSONs string and returns them line by line, sorted by key
def main():
    data = ''.join(sys.stdin.readlines())
    #for line in sys.stdin.readlines():
    jsonObject = json.loads(data)
    print json.dumps(jsonObject, sort_keys=True, separators=(',', ':'))
    
if __name__ == "__main__": 
    main()
