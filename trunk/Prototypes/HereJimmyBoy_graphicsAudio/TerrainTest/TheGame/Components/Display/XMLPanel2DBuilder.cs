using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace TheGame.Components.Display
{

    /// <summary>
    /// Parses an XML Document and builds a panel object from an xml file. Handy for menus.
    /// 
    /// Here is an example of a panel with an image.
    ///<panel>
    ///  <position>
    ///    <xpos>250</xpos>
    ///    <ypos>200</ypos>
    /// </position>
    ///    <image2d>
    ///        <xpos>0</xpos>
    ///       <ypos>0</ypos>
    ///        <name>menubg</name>
    ///        <rcolor>255</rcolor>
    ///        <gcolor>255</gcolor>
    ///       <bcolor>255</bcolor>
    ///    </image2d>
    ///</panel>
    /// </summary>

    class XMLPanel2DBuilder
    {
        private GameScreen parent;

        private ContentManager content;

        private string file;

        private XmlDocument document = new XmlDocument();

        private PanelComponent2D panel = null;

        public PanelComponent2D Panel
        {
            get { return panel; }
        }

        /// <summary>
        /// Creates a XML Panel builder creates PanelComponent2D objects from an XML file.
        /// </summary>
        /// <param name="content">Needed for loading images</param>
        /// <param name="file">XML file</param>
        /// <param name="parseNow">parse on creation if true else don't</param>
        public XMLPanel2DBuilder(GameScreen parent, ContentManager content, string file, bool parseNow)
        {
            this.parent = parent;
            this.content = content;
            this.file = file;
            this.document.Load(file);

            if (parseNow)
            {
                ParseDocument();
            }
        }

        /// <summary>
        /// Creates a XML Panel builder creates PanelComponent2D objects from an XML file. Parses XML on creation.
        /// </summary>
        /// <param name="content">Needed for loading images</param>
        /// <param name="file">XML file</param>
        /// <param name="parseNow">parse on construction if true else don't</param>
        public XMLPanel2DBuilder(GameScreen parent, ContentManager content, string file)
            : this(parent, content, file, true)
        {

        }

        /// <summary>
        /// Parses the xml document and builds the panel from it.
        /// </summary>
        public void ParseDocument()
        {

            XmlNodeList root = document.GetElementsByTagName("panel");

            XmlNodeList components = root.Item(0).ChildNodes;

            GetPanelPosition();

            if (panel != null)
            {
                foreach (XmlNode component in components)
                {
                    switch (component.Name)
                    {
                        case "text2d":
                            this.panel.PanelItems.Add(CreateTextComponent2d(component.ChildNodes));
                            break;
                        case "image2d":
                            this.panel.PanelItems.Add(CreateImageComponent2d(component.ChildNodes));
                            break;
                    }
                }
            }
        }

        private void GetPanelPosition()
        {
            XmlNode pos = document.GetElementsByTagName("position").Item(0);
            PanelComponent2D temp = null;
            XmlNodeList attributes = pos.ChildNodes;
            if (attributes.Count == 2)
            {
                string xpos = attributes.Item(0).InnerText,
                ypos = attributes.Item(1).InnerText;
                Vector2 position = new Vector2(Int16.Parse(xpos), Int16.Parse(ypos));
                temp = new PanelComponent2D(parent, position);
            }
            panel = temp;
        }

        private ImageComponent2D CreateImageComponent2d(XmlNodeList attributes)
        {
            ImageComponent2D image2d = null;

            if (attributes.Count == 6)
            {
                //Extract position
                string xpos = attributes.Item(0).InnerText,
                    ypos = attributes.Item(1).InnerText,

                    //Extract image name
                    imagename = attributes.Item(2).InnerText,

                    //Extract tint color
                    rcolor = attributes.Item(3).InnerText,
                    gcolor = attributes.Item(4).InnerText,
                    bcolor = attributes.Item(5).InnerText;

                //Parse position, color and load texture
                Vector2 position = new Vector2(Int16.Parse(xpos), Int16.Parse(ypos));
                Color color = new Color(Byte.Parse(rcolor), Byte.Parse(gcolor), Byte.Parse(bcolor));
                Texture2D texture = content.Load<Texture2D>(imagename);

                image2d = new ImageComponent2D(parent, position, texture, color);
            }


            return image2d;
        }

        private TextComponent2D CreateTextComponent2d(XmlNodeList attributes)
        {
            TextComponent2D text2d = null;

            if (attributes.Count == 7)
            {
                //Extract position
                string xpos = attributes.Item(0).InnerText,
                    ypos = attributes.Item(1).InnerText,

                    //Extract image name
                    text = attributes.Item(2).InnerText,

                    //Extract tint color
                    rcolor = attributes.Item(3).InnerText,
                    gcolor = attributes.Item(4).InnerText,
                    bcolor = attributes.Item(5).InnerText,

                    fontname = attributes.Item(6).InnerText;

                //Parse position, color and load font
                Vector2 position = new Vector2(Int16.Parse(xpos), Int16.Parse(ypos));
                Color color = new Color(Byte.Parse(rcolor), Byte.Parse(gcolor), Byte.Parse(bcolor));
                SpriteFont font = content.Load<SpriteFont>(fontname);

                text2d = new TextComponent2D(parent, position, text, color, font);
            }

            return text2d;
        }
    }
}
