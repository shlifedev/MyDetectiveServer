
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
namespace GameTable.Buff
{
    public class Info
    {
static bool isLoaded = false;
	public static List<Info>       list       = new List<Info>();
	public static Dictionary<int, Info> dict       = new Dictionary<int, Info>();
	public int Index;
	public string Name;
	public float BuffTime;
	public List<int> EndNextBuff;
	public bool Removeable;
	public bool IsPublic;
	public bool Stackable;
	public EBuffType BuffType;
	public bool Tradeable;
	public string SpritePath;
 

#if !SERVER
        public static void Load()
        {
if(isLoaded) return;
isLoaded = true;
          var textAsset = Resources.Load("TableDatas/GameTable.Buff.Info") as TextAsset;
          var str = textAsset.text; 
          var loadedList = JsonConvert.DeserializeObject<List<Info>>(str);
          for(int i = 0; i < loadedList.Count; i++)
          {
            
              var data = loadedList[i];
              if(loadedList != null)
              {
                    list.Add(loadedList[i]);
                    dict.Add(loadedList[i].Index, loadedList[i]);
              }
          }
    
        }
#else
        public static void Load()
        { 
if(isLoaded) return;
isLoaded = true;
          var str = File.ReadAllText("TableDatas/GameTable.Buff.Info" + ".txt");
          var loadedList = JsonConvert.DeserializeObject<List<Info>>(str);
          for(int i = 0; i < loadedList.Count; i++)
          {
            
              var data = loadedList[i];
              if(loadedList != null)
              {
                    list.Add(loadedList[i]);
                    dict.Add(loadedList[i].Index, loadedList[i]);
              }
          }
    
        }

#endif
 

        public static Info Get(int index)
        {
           if(list.Count == 0) Load();
           bool exist = dict.ContainsKey(index);
           if(exist)
              return dict[index];
           else
              return null;
        }

    }
}
            