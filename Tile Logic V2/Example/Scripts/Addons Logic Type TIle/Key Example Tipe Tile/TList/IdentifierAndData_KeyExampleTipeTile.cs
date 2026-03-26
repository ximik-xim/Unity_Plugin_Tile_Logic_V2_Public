using UnityEngine; 
using TListPlugin; 
[System.Serializable]
public class IdentifierAndData_KeyExampleTipeTile : AbsIdentifierAndData<IndifNameSO_KeyExampleTipeTile, string, KeyExampleTipeTile>
{

 [SerializeField] 
 private KeyExampleTipeTile _dataKey;


 public override KeyExampleTipeTile GetKey()
 {
  return _dataKey;
 }
 
#if UNITY_EDITOR
 public override string GetJsonSaveData()
 {
  return JsonUtility.ToJson(_dataKey);
 }

 public override void SetJsonData(string json)
 {
  _dataKey = JsonUtility.FromJson<KeyExampleTipeTile>(json);
 }
#endif
}
