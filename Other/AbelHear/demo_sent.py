import requests
import json
SERVER_PATH = "http://api.italianlp.it"

texts = ["Secondo me devi imparare ancora molto", "sono sicuro che in futuro andrà molto meglio", "non ti preoccupare se hai sbagliato, la prossima volta darai risposte giuste", "la risposta che hai dato non è corretta", "le domande erano troppo difficili"]

for text in texts:

	r = requests.post(SERVER_PATH + '/documents/',
		              data={'text': text,
		                    'lang': "IT",
		                    'extra_tasks': ["sentiment"]
		              })

	id_doc = r.json()['id']
	r = requests.get(SERVER_PATH + '/documents/details/%s' % id_doc)
	data = r.json()

	sentiment_negative_probability = data['sentiment_negative_probability']
	sentiment_positive_probability = data['sentiment_positive_probability']
	sentiment_positive_negative_probability = data['sentiment_positive_negative_probability']
	sentiment_neutral_probability = data['sentiment_neutral_probability']

	print(text)

	print('NEG:', sentiment_negative_probability)
	print('POS:', sentiment_positive_probability)
	print('POS_NEG:', sentiment_positive_negative_probability)
	print('NEU:', sentiment_neutral_probability)
	
	print('\n\n')
