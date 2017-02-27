	using System.Collections.Generic;
	using UnityEngine.Networking;
	using UnityEngine;
	using System.IO;
	using System;
	using System.Linq;
	using System.Collections;

public class url_demo : MonoBehaviour {
	string[] lines = System.IO.File.ReadAllLines("Assets/UrlFile.txt");
	int i=0;
	//Vector3 camView = new Vector3(6.17f, 2.25f, -6.14f);
	//SortedList mySL = new SortedList();
	//Dictionary dictionary = new Dictionary<string, int>();
	Hashtable hashtable = new Hashtable();
	void Start () {
//		camView = GameObject.Find("Main Camera").transform.position;
		foreach (string line in lines)
		{
			print (line);
			var watch = System.Diagnostics.Stopwatch.StartNew();
			StartCoroutine(GetText());
			double elapsedMs = watch.ElapsedMilliseconds;
			watch.Stop();

			print (elapsedMs);
			i++;
			hashtable.Add (line,elapsedMs);
			//mySL.Add(line,elapsedMs);
			//dictionary.Add(line, elapsedMs);

		}
			
		/*for ( int i = 0; i < mySL.Count; i++ )  {
			print( "Key= "+ mySL.GetKey(i) +"Value="+ mySL.GetByIndex(i));
		}*/
			
		//hashtable.Cast<DictionaryEntry>().OrderBy(entry => entry.Value).ToList()
		float x = 0.5F;

		foreach (DictionaryEntry entry in hashtable)
		{
			print( "Key= "+ entry.Key +"Value="+ entry.Value);
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.name = Convert.ToString (entry.Key);
			int y = Convert.ToInt16 (entry.Value);
			sphere.transform.position = new Vector3(10F+x,10, 10);
			//transform.localScale += new Vector3(x+0.1F, x+0.1F, x+0.1F);
			sphere.transform.localScale = new Vector3(y, y, y);

			x++;
		}
			
	

	}

	IEnumerator GetText() {
		using(UnityWebRequest www = UnityWebRequest.Get(lines[i])) {
			print ("GET REQUEST SENT");
			yield return www.Send();
			//print (www.Send());
			if(www.isError) {
				Debug.Log(www.error);
			}
			else {
				// Show results as text
				Debug.Log(www.downloadHandler.text);

				// Or retrieve results as binary data
				byte[] results = www.downloadHandler.data;
			}
		}
	}
}

	
	

