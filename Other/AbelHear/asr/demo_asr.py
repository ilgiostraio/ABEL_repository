#!/usr/bin/env python3

import itanlp_sr as sr

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

