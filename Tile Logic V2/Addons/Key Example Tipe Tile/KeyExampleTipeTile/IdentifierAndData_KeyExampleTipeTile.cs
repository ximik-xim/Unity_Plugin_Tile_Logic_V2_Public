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
}
