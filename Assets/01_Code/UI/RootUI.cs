using System.Collections.Generic;
using System.Linq;
using KWJ.Code.Define;
using UnityEngine;

namespace KWJ.Code.UI
{
    public class RootUI : MonoBehaviour
    {
        public Dictionary<PanelType, RootUIPanel> PanelDict { get; private set; }

        private List<RootUIPanel> _onPanels = new List<RootUIPanel>();
        private List<RootUIPanel> _offPanels = new List<RootUIPanel>();

        private void Awake()
        {
            PanelDict = new Dictionary<PanelType, RootUIPanel>();
            AddPanelComponent();

            foreach (var panel in PanelDict)
            {
                _offPanels.Add(panel.Value);
            }
            
            //OnOffPanels(GetPanel(PanelType.InventoryBar));
        }
        
        public RootUIPanel GetPanel(PanelType panelType)
        {
            return PanelDict.GetValueOrDefault(PanelType.InventoryBar);
        }

        private void AddPanelComponent()
        {
            GetComponentsInChildren<RootUIPanel>(true).ToList().ForEach(panel =>
                PanelDict[panel.PanelType] = panel);
        }
        
        private void OnOffPanels(RootUIPanel panel)
        {
            if (_onPanels.Contains(panel))
            {
                panel.gameObject.SetActive(false);
                _onPanels.Remove(panel);
                _offPanels.Add(panel);
            }
            else
            {
                panel.gameObject.SetActive(true);
                _offPanels.Remove(panel);
                _onPanels.Add(panel);
            }
        }

    }
}