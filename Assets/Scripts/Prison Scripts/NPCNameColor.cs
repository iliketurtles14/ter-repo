using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NPCNameColor : MonoBehaviour
{
	//guard: 115, 196, 243
	private List<int> colorrefs = new List<int>
	{
		255,
		26367,
		39423,
		52479,
		56575,
		65535,
		65484,
		65433,
		64614,
		65280
	};
	private List<Color> colors = new List<Color>();
	public Color currentColor;
	private NPCCollectionData npcColData;
	private Color guardColor = new Color(115f / 255f, 196f / 255f, 243f / 255f);
	private void Start()
	{
		npcColData = GetComponent<NPCCollectionData>();
		MakeColors();
	}
	private void MakeColors()//cuz i dont want to decode these color refs myself
	{
		foreach(int cRef in colorrefs)
		{
			int r = cRef & 0xFF;
			int g = (cRef >> 8) & 0xFF;
			int b = (cRef >> 16) & 0xFF;
			Color color = new Color(r / 255f, g / 255f, b / 255f); 
			colors.Add(color);
		}
	}
	private void Update()
	{
		if (name.Contains("Inmate"))
		{
			for (int i = 9; i >= 0; i--)
			{
				if (npcColData.npcData.opinion >= i * 10)
				{
					currentColor = colors[i];
					break;
				}
			}
		}
		else
		{
			currentColor = guardColor;
		}
	}
}
