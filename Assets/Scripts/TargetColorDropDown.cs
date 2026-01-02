using TMPro;
using UnityEngine;
using System;

public class TargetColorDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    public PieceColor SelectedColor { get; private set; }

    private void Awake()
    {
        dropdown.ClearOptions();

        var names = Enum.GetNames(typeof(PieceColor));
        dropdown.AddOptions(new System.Collections.Generic.List<string>(names));

        dropdown.onValueChanged.AddListener(OnValueChanged);

        // mặc định chọn 0
        SelectedColor = (PieceColor)0;
    }

    private void OnValueChanged(int index)
    {
        SelectedColor = (PieceColor)index;
    }
}
