#!/usr/bin/env python3

import itanlp_sr as sr
import numpy
import yarp
 

# mettere nella cartella la chiave json per usare l'API (o indicarne il percorso) 
GOOGLE_CLOUD_SPEECH_CREDENTIALS = open("key.json", 'r').read()


with sr.Microphone() as source:
	r = sr.Recognizer()
	r.adjust_for_ambient_noise(source)

	while True:
			print("Parla pure!")
			# listen for 1 second to calibrate the energy threshold for ambient noise levels
			audio = r.listen(source)

			response = r.recognize_google_cloud(audio, language='it-IT',
					                            credentials_json=GOOGLE_CLOUD_SPEECH_CREDENTIALS,
					                            show_all=True)
			try:
				text = response['results'][0]['alternatives'][0]['transcript']
				print(text)
			except:
				print('-------------')



# Create the yarp port, connect it to the running instance of yarpview and send the data
yarp.Network.init()
output_port = yarp.Port()
input_port = yarp.Port()
output_port.open("/abel_asr:o")
input_port.open("/abel_asr:i")
yarp.Network.connect("/abel:o", "/abel:i")
output_port.write('text')
 
# Cleanup
output_port.close()

