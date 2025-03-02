using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveAndLoadSetting
{
    void SaveSetting(ref GameSettingData gameSettingData);
    void LoadSetting(GameSettingData gameSettingData);
}
