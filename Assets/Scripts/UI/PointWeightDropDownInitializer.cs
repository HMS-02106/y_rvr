using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtility.Enums;
using UnityUtility.Linq;

public class PointWeightDropDownInitializer : MonoBehaviour
{
    void Start()
    {
        var dropdownOptions = EnumUtils.All<PointWeight>()
            .Select(w => new TMP_Dropdown.OptionData(w.ToString()))
            .ToList();
        GetComponent<TMP_Dropdown>().options = dropdownOptions;
    }
}
