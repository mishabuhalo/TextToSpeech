# -*- coding: utf-8 -*-


import wave
import contextlib
import json

import parts


PATH = "../Resources/Sounds/"
EXTENSION = ".wav"
OUTPUT = "../Resources/Sounds.wav"
PARAMS = (1, 2, 44100, 0, 'NONE', 'not compressed')
RESULT = "../Resources/Sounds.json"


def main():
    print "Processing started"

    result = []

    with contextlib.closing(wave.open(OUTPUT, "w")) as outfile:
        outfile.setparams(PARAMS)
        position = 0

        for part in parts.PARTS:
            if part["play"]:
                filename = PATH + part["play"] + EXTENSION
                with contextlib.closing(wave.open(filename, "r")) as infile:
                    frames = infile.getnframes()
                    rate = infile.getframerate()
                    duration = frames * 1000 / rate

                    outfile.writeframes(infile.readframes(frames))
                    result.append({
                        "Part": part["part"],
                        "Start": position,
                        "Duration": duration
                    })
                    position = outfile.getnframes() * 1000 / outfile.getframerate()
            else:
                result.append({
                    "Part": part["part"],
                    "Start": -1,
                    "Duration": part["wait"]
                })

    with open(RESULT, "w") as r:
        r.write(json.dumps(result, indent=4, ensure_ascii=False))
        
    print "Resources contain " + str(len(result)) + " parts"

    print "Processing finished"


if __name__ == "__main__":
    main()
