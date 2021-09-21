using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LordAshes
{
    public partial class GUIMenuPlugin : BaseUnityPlugin
    {

        public interface IMenuItem
        {
            string title { get; set; }
            UnityEngine.Color color { get; set; }
            Texture2D icon { get; set; }
            bool gmOnly { get; set; }
        }

        public class MenuSelection : IMenuItem
        {
            public MenuSelection()
            {
            }

            public MenuSelection(string selection, bool gmOnly = false)
            {
                this.selection = selection;
                this.title = selection;
                this.color = UnityEngine.Color.black;
                this.icon = null;
                this.gmOnly = gmOnly;
            }

            public MenuSelection(string selection, string title, bool gmOnly = false)
            {
                this.selection = selection;
                this.title = title;
                this.color = UnityEngine.Color.black;
                this.icon = null;
                this.gmOnly = gmOnly;
            }

            public MenuSelection(string selection, string title, UnityEngine.Color color, bool gmOnly = false)
            {
                this.selection = selection;
                this.title = title;
                this.color = color;
                this.icon = null;
                this.gmOnly = gmOnly;
            }

            public MenuSelection(string selection, Texture2D icon, bool gmOnly = false)
            {
                this.selection = selection;
                this.title = "";
                this.color = UnityEngine.Color.black;
                this.icon = icon;
                this.gmOnly = gmOnly;
            }

            public MenuSelection(string selection, string title, Texture2D icon, bool gmOnly = false)
            {
                this.selection = selection;
                this.title = title;
                this.color = UnityEngine.Color.black;
                this.icon = icon;
                this.gmOnly = gmOnly;
            }

            public MenuSelection(string selection, string title, UnityEngine.Color color, Texture2D icon, bool gmOnly = false)
            {
                this.selection = selection;
                this.title = title;
                this.color = color;
                this.icon = icon;
                this.gmOnly = gmOnly;
            }

            public string title { get; set; } = "";
            public UnityEngine.Color color { get; set; }

            public Texture2D icon { get; set; } = null;
            public string selection { get; set; } = System.Guid.NewGuid().ToString();
            public bool gmOnly { get; set; }
        }

        public class MenuLink : IMenuItem
        {
            public MenuLink()
            {
            }

            public MenuLink(string link, bool gmOnly = false)
            {
                this.link = link;
                this.title = link;
                this.color = UnityEngine.Color.black;
                this.icon = null;
                this.gmOnly = gmOnly;
            }

            public MenuLink(string link, string title, bool gmOnly = false)
            {
                this.link = link;
                this.title = title;
                this.color = UnityEngine.Color.black;
                this.icon = null;
                this.gmOnly = gmOnly;
            }

            public MenuLink(string link, string title, UnityEngine.Color color, bool gmOnly = false)
            {
                this.link = link;
                this.title = title;
                this.color = color;
                this.icon = null;
                this.gmOnly = gmOnly;
            }

            public MenuLink(string link, Texture2D icon, bool gmOnly = false)
            {
                this.link = link;
                this.title = "";
                this.color = UnityEngine.Color.black;
                this.icon = icon;
                this.gmOnly = gmOnly;
            }

            public MenuLink(string link, string title, Texture2D icon, bool gmOnly = false)
            {
                this.link = link;
                this.title = title;
                this.color = UnityEngine.Color.black;
                this.icon = icon;
                this.gmOnly = gmOnly;
            }

            public MenuLink(string link, string title, UnityEngine.Color color, Texture2D icon, bool gmOnly = false)
            {
                this.link = link;
                this.title = title;
                this.color = color;
                this.icon = icon;
                this.gmOnly = gmOnly;
            }

            public string title { get; set; } = "";
            public UnityEngine.Color color { get; set; }

            public Texture2D icon { get; set; } = null;
            public string link { get; set; } = "";
            public bool gmOnly { get; set; }
        }

        public enum MenuStyle
        {
            centre = 0,
            side
        }

        public enum MenuState
        {
            closed = 0,
            opening,
            opened,
            closing
        }

        public class MenuNode
        {
            private List<IMenuItem> _items = new List<IMenuItem>();
            public string name { get; set; } = System.Guid.NewGuid().ToString();
            public MenuStyle style { get; set; } = MenuStyle.centre;
            private MenuState state = MenuState.closed;
            public int position { get; set; } = 0;

            public MenuNode()
            {

            }

            public MenuNode(string name, IMenuItem[] items, MenuStyle style = MenuStyle.centre)
            {
                this.name = name;
                this._items = items.ToList<IMenuItem>();
                this.style = style;
            }

            public void AddSelection(string selection, string title = "", Texture2D icon = null)
            {
                _items.Add(new MenuSelection() { selection = selection, title = title, icon = icon });
            }

            public void AddSelection(MenuSelection selection)
            {
                _items.Add(selection);
            }

            public void AddLink(string link, string title = "", Texture2D icon = null)
            {
                _items.Add(new MenuLink() { link = link, title = title, icon = icon });
            }

            public void AddLink(MenuLink link)
            {
                _items.Add(link);
            }

            public IMenuItem[] GetItems()
            {
                return _items.ToArray();
            }

            public MenuState GetState()
            {
                return state;
            }

            public void SetState(MenuState state)
            {
                this.state = state;
            }

            public void Animate()
            {
                if (state == MenuState.opening) { position = position + 4; if (position >= 80) { state = MenuState.opened; } }
                else if (state == MenuState.closing) { position = position - 4; if (position <= 0) { state = MenuState.closed; } }
            }
        }

        public class GuiMenu
        {
            private Dictionary<string, MenuNode> _nodes = new Dictionary<string, MenuNode>();
            private Action<string> _callback = null;
            private MenuNode _currentNode = null;
            private string _queueNodeName = null;
            private int animationSpeed = 1;

            public GuiMenu()
            {

            }

            public GuiMenu(MenuNode[] nodes)
            {
                foreach (MenuNode node in nodes)
                {
                    this._nodes.Add(node.name, node);
                }
            }

            public void AddNode(MenuNode node)
            {
                this._nodes.Add(node.name, node);
            }

            public IMenuItem[] GetNodeItems(string node)
            {
                if (_nodes.ContainsKey(node)) { return _nodes[node].GetItems(); } else { return null; }
            }

            public MenuNode GetNode(string node)
            {
                if (_nodes.ContainsKey(node)) { return _nodes[node]; } else { return null; }
            }

            public MenuNode GetCurrentNode()
            {
                return _currentNode;
            }

            public void Open(string node, Action<string> callback)
            {
                if (this.GetNode(node) != null)
                {
                    _callback = callback;
                    this.Open(node);
                }
            }

            public void Open(string node)
            {
                if (this.GetNode(node) != null)
                {
                    _currentNode = this.GetNode(node);
                    if (_currentNode.style == MenuStyle.centre) { _currentNode.SetState(MenuState.opened); } else { _currentNode.SetState(MenuState.opening); }
                    _currentNode.SetState(MenuState.opening);
                }
            }

            public void Queue(string node)
            {
                if (this.GetNode(node) != null)
                {
                    _queueNodeName = node;
                    if (_currentNode.style == MenuStyle.centre) { _currentNode.SetState(MenuState.closed); } else { _currentNode.SetState(MenuState.closing); }
                }
            }

            public void Draw()
            {
                if (this.GetCurrentNode() != null)
                {
                    if (this.GetCurrentNode().GetState() == MenuState.closed && this._queueNodeName != null)
                    {
                        Debug.Log("GUI Menu Plugin: Processing Queue Menu '" + this._queueNodeName + "'");
                        this.Open(this._queueNodeName); _queueNodeName = null;
                    }
                    if ((this.GetCurrentNode().GetState() != MenuState.closed) || (_queueNodeName != null))
                    {
                        int offsetPerItem = 70;
                        int offsetX;
                        int offsetY;
                        int offsetLabelX;
                        int offsetLabelY;
                        int offsetCount = 0;
                        int optionsCount = 0;
                        foreach (IMenuItem item in this.GetCurrentNode().GetItems())
                        {
                            if(item.gmOnly == false || LocalClient.IsInGmMode) { optionsCount++; }
                        }

                        foreach (IMenuItem item in this.GetCurrentNode().GetItems())
                        {
                            if (item.gmOnly == false || LocalClient.IsInGmMode)
                            {
                                GUIStyle style = new GUIStyle();
                                style.fontStyle = FontStyle.Bold;
                                style.normal.textColor = item.color;
                                switch (this.GetCurrentNode().style)
                                {
                                    case MenuStyle.side:
                                        if (this.GetCurrentNode().GetState() == MenuState.opening) { this.GetCurrentNode().position = this.GetCurrentNode().position + animationSpeed; if (this.GetCurrentNode().position >= 80) { this.GetCurrentNode().SetState(MenuState.opened); }; }
                                        if (this.GetCurrentNode().GetState() == MenuState.closing) { this.GetCurrentNode().position = this.GetCurrentNode().position - animationSpeed; if (this.GetCurrentNode().position <= 0) { this.GetCurrentNode().SetState(MenuState.closed); }; }
                                        offsetX = 1920 - this.GetCurrentNode().position;
                                        offsetY = (1080 - optionsCount * offsetPerItem) / 2;
                                        offsetY = offsetY + offsetCount * offsetPerItem;
                                        offsetLabelX = -offsetPerItem;
                                        offsetLabelY = (offsetPerItem / 4);
                                        style.alignment = TextAnchor.MiddleRight;
                                        break;
                                    default:
                                        offsetX = (1920 - (optionsCount * offsetPerItem)) / 2;
                                        offsetX = offsetX + offsetCount * offsetPerItem;
                                        offsetY = (1080 - 70) / 2;
                                        offsetLabelX = 0;
                                        offsetLabelY = offsetPerItem;
                                        style.alignment = TextAnchor.MiddleCenter;
                                        break;
                                }
                                if (item.GetType().Name == typeof(MenuSelection).Name)
                                {
                                    if (GUI.Button(new Rect(offsetX, offsetY, 68, 68), ""))
                                    {
                                        Debug.Log("GUI Menu Plugin: Selected Item '" + ((MenuSelection)item).selection + "'");
                                        this.Close();
                                        string selection = ((MenuSelection)item).selection;
                                        _callback(selection);
                                    }
                                    GUI.DrawTexture(new Rect(offsetX, offsetY, 68, 68), item.icon);
                                    GUI.Label(new Rect(offsetX + offsetLabelX, offsetY + offsetLabelY, 68, 32), item.title, style);
                                }
                                else
                                {
                                    if (GUI.Button(new Rect(offsetX, offsetY, 68, 68), ""))
                                    {
                                        Debug.Log("GUI Menu Plugin: Displaying Menu '" + ((MenuLink)item).link + "'");
                                        this.Queue(((MenuLink)item).link);
                                    }
                                    GUI.DrawTexture(new Rect(offsetX, offsetY, 68, 68), item.icon);
                                    GUI.Label(new Rect(offsetX + offsetLabelX, offsetY + offsetLabelY, 68, 32), item.title, style);
                                }
                                offsetCount++;
                            }
                        }
                    }
                }
            }

            public void Close()
            {
                if (_currentNode != null)
                {
                    if (_currentNode.GetState() != MenuState.closed)
                    {
                        if (_currentNode.style == MenuStyle.centre) { _currentNode.SetState(MenuState.closed); } else { _currentNode.SetState(MenuState.closing); }
                    }
                }
            }
        }
    }
}
