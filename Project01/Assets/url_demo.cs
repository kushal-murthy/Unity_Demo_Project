/*
This code plots a sphere for each URL on the unity scene. The spheres are placed in sorted order of time on x-axis and are scaled as per the response size. 
There are 20 URL's in the UrlFile.txt file.
The time taken to fulfil a request is measured naively using System timer without using any high level API.
The size is measured as per the length of HTTP response data.
Date Created: 27th February
Author:Kushal Vangara

Modified on 28th February 
*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
public class url_demo : MonoBehaviour {

	GameObject mainCamera;
	string[] lines = System.IO.File.ReadAllLines("Assets/UrlFile.txt"); //URL's source file
	SortedDictionary<float, string> urls = new SortedDictionary<float,string> (); //map time, url name and response size
	byte[] results; //response bytes
	string[] urlsLine; //array for url's
	int i=0, y=0,f=0; //counters and scaling variable
	float x = 10F;   // x-axis padding spacing between spheres
	float elapsedMs=0f, temp=0f;  // variables for calculating time
	bool flag=false; 

	void Start () {
		urlsLine = new string[lines.Length];
		mainCamera = GameObject.Find ("Main Camera");
		foreach (string line in lines)
		{
			urlsLine [i] = line;
			StartCoroutine (GetText ());
			i++;

		}
		mainCamera.transform.position = new Vector3(788, 876, -14); // default camera view 

		mainCamera.transform.rotation = Quaternion.Euler(90, 0, 0);  
	  }

	IEnumerator GetText() {
			temp = Time.time;
			using(UnityWebRequest www = UnityWebRequest.Get(urlsLine[i])) {
				print ("GET Request Sending to URL's...");
				yield return www.Send();
				
				if(www.isError) {
					Debug.Log(www.error);
				}

				else {
					results = www.downloadHandler.data;
				}

				urls.Add (elapsedMs, urlsLine[f]+"\t"+results.Length.ToString()); 
				elapsedMs = (Time.time - temp) * 1000;
				print ("Url #"+ (f+1) + " is " + urlsLine[f]+"  Time= "+elapsedMs+"  Size="+results.Length);
				f++;

				if (f == urlsLine.Length) { //to iterate over all the url's then plot the spheres
					flag = true;
					print ("Task Finished");
				}

			}

		}

	void LateUpdate(){

		if (flag) {
			foreach (var kvp in urls) {
					//print ("Key= " + kvp.Key + "Value=" + kvp.Value);
					GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
					sphere.name = Convert.ToString (kvp.Value);
					int index = kvp.Value.IndexOf ("\t");
					string subs = kvp.Value.Substring (index + 1);
					y = (int.Parse (subs));

					//Normalizing scaling factor w.r.t the response size 
					if (y >= 200000)
						y = 50;
					else if (y >= 100000)
						y = 40;
					else if (y >= 50000)
						y = 33;
					else if (y >= 10000)
						y = 25;
					else if (y >= 5000)
						y = 18;
					else if (y > 1000)
						y = 13;
					else
						y = 10;
				

				sphere.transform.position = new Vector3 (x, 100, 20);
				sphere.GetComponent<Renderer> ().material.color = new Color (0, 0, 255F);
				sphere.transform.localScale = new Vector3(y, y, y);
				x = x + 80;
			}

			flag = false;
		}

	}

}
