using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SkillConfigTable", menuName = "SkillConfig/SkillConfigTable")]
public class SkillConfigTable : ScriptableObject
{
    public SkillConfig[] skillConfigs;
}
