﻿using System;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using uComponents.Core;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.property;
using umbraco.cms.businesslogic.web;
using umbraco.interfaces;
using Umbraco.Web;
using umbraco.editorControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web;
using System.Drawing;
using Image = System.Web.UI.WebControls.Image;
using DefaultData = umbraco.cms.businesslogic.datatype.DefaultData;

[assembly: WebResource("uComponents.DataTypes.ImagePoint.ImagePoint.css", Constants.MediaTypeNames.Text.Css)]
[assembly: WebResource("uComponents.DataTypes.ImagePoint.ImagePoint.js", Constants.MediaTypeNames.Application.JavaScript)]
[assembly: WebResource("uComponents.DataTypes.ImagePoint.ImagePointMarker.png", Constants.MediaTypeNames.Image.Png)]
namespace uComponents.DataTypes.ImagePoint
{
    /// <summary>
    /// Image Point Data Type
    /// </summary>
    [ClientDependency.Core.ClientDependency(ClientDependency.Core.ClientDependencyType.Javascript, "ui/jqueryui.js", "UmbracoClient")]
    public class ImagePointDataEditor : CompositeControl, IDataEditor
    {
        /// <summary>
        /// Field for the data.
        /// </summary>
        private IData data;

        /// <summary>
        /// Field for the options.
        /// </summary>
        private ImagePointOptions options;

        /// <summary>
        /// Wrapping div
        /// </summary>
        private HtmlGenericControl div = new HtmlGenericControl("div");

        /// <summary>
        /// X coordinate
        /// </summary>
        private TextBox xTextBox = new TextBox();

        /// <summary>
        /// Y coordinate
        /// </summary>
        private TextBox yTextBox = new TextBox();

        /// <summary>
        /// Image tag used to define the x, y area
        /// </summary>
        private Image mainImage = new Image();

        /// <summary>
        /// Image used to mark the point
        /// </summary>
        private Image markerImage = new Image();

        /// <summary>
        /// Initializes a new instance of the <see cref="ImagePointDataEditor"/> class. 
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="options">The options.</param>
        internal ImagePointDataEditor(IData data, ImagePointOptions options)
        {
            this.data = data;
            this.options = options;
        }

        /// <summary>
        /// Gets a value indicating whether [treat as rich text editor].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [treat as rich text editor]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool TreatAsRichTextEditor
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [show label].
        /// </summary>
        /// <value><c>true</c> if [show label]; otherwise, <c>false</c>.</value>
        public virtual bool ShowLabel
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the editor.
        /// </summary>
        /// <value>The editor.</value>
        public Control Editor
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Gets the id of the current (content, media or member) on which this is a property
        /// </summary>
        private int CurrentContentId
        {
            get
            {
                return ((DefaultData)this.data).NodeId;
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            /*  
             * 
             *  <div class="image-point">
             *      <input type="text" class="x" />
             *      <input type="text" class="y" />
             *      <div class="area">
             *          <img src="" width="" height="" class="main" />
             *          <ing src="" class="marker" />
             *      </div>
             *  </div>
             * 
             */
            this.div.Attributes.Add("class", "image-point");

            this.xTextBox.ID = "xTextBox";
            this.xTextBox.CssClass = "x";
            this.xTextBox.Width = 30;
            this.xTextBox.MaxLength = 4;                        

            this.yTextBox.ID = "yTextBox";
            this.yTextBox.CssClass = "y";
            this.yTextBox.Width = 30;
            this.yTextBox.MaxLength = 4;

            WebControl areaDiv = new WebControl(HtmlTextWriterTag.Div);
            areaDiv.CssClass = "area";
            areaDiv.Controls.Add(this.mainImage);
            areaDiv.Controls.Add(this.markerImage);

            this.mainImage.CssClass = "main";           

            this.markerImage.CssClass = "marker";
            this.markerImage.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), "uComponents.DataTypes.ImagePoint.ImagePointMarker.png");

            this.div.Controls.Add(new Literal() { Text = "X " });
            this.div.Controls.Add(this.xTextBox);
            this.div.Controls.Add(new Literal() { Text = " Y " });
            this.div.Controls.Add(this.yTextBox);
            this.div.Controls.Add(new Literal() { Text = "<br />" });
            this.div.Controls.Add(areaDiv);

            this.Controls.Add(this.div);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.EnsureChildControls();

            this.CalculateWidthHeight();

            if (!this.Page.IsPostBack && this.data.Value != null)
            {
                // set the x and y textboxes

                // the ImagePoint class is usually be used by uQuery: eg. uQuery.GetCurrentNode().GetProperty<ImagePoint>("alias").X;
                ImagePoint value = new ImagePoint();
                ((uQuery.IGetProperty)value).LoadPropertyValue(this.data.Value.ToString());

                if (value.X != null)
                {
                    this.xTextBox.Text = value.X.ToString();
                }

                if (value.Y != null)
                {
                    this.yTextBox.Text = value.Y.ToString();
                }

                // replaced with the uQuery.GetProperty type method above
                //string[] coordinates = this.data.Value.ToString().Split(',');
                //if (coordinates.Length == 2)
                //{
                //    this.xTextBox.Text = coordinates[0];
                //    this.yTextBox.Text = coordinates[1];
                //}
            }

            this.RegisterEmbeddedClientResource("uComponents.DataTypes.ImagePoint.ImagePoint.css", ClientDependencyType.Css);
            this.RegisterEmbeddedClientResource("uComponents.DataTypes.ImagePoint.ImagePoint.js", ClientDependencyType.Javascript);

            string startupScript = @"
                <script language='javascript' type='text/javascript'>
                    $(document).ready(function () {
                        ImagePoint.init(jQuery('div#" + this.div.ClientID + @"'));
                    });
                </script>";

            ScriptManager.RegisterStartupScript(this, typeof(ImagePointDataEditor), this.ClientID + "_init", startupScript, false);
        }

        /// <summary>
        /// Called by Umbraco when saving the node
        /// </summary>
        public void Save()
        {
            this.data.Value = this.xTextBox.Text + "," + this.yTextBox.Text;
        }

        /// <summary>
        /// Calculates the width and height and sets these as data-attributes on the containing div (as well as on the image)
        /// if a width or height value has been supplied then these are used in preference to the image dimensions
        /// if a width or height value has been supplied, and one of these = 0, then it needs to be calculated from the image dimensions (so as to keep the aspect ratio)
        /// </summary>
        private void CalculateWidthHeight()
        {            
            int width = 0; // default unknown values
            int height = 0;
            Size imageSize = new Size(0, 0);
           
            if (!string.IsNullOrWhiteSpace(this.options.ImagePropertyAlias))
            {
                string imageUrl = null;

                // used large try/catch block to simply checking for nulls on content / media & members
                try
                {
                    // looking for the specified property
                    switch (uQuery.GetUmbracoObjectType(this.CurrentContentId))
                    {
                        case uQuery.UmbracoObjectType.Document:
                            imageUrl = uQuery.GetDocument(this.CurrentContentId)
                                                .GetAncestorOrSelfDocuments()
                                                .First(x => x.HasProperty(this.options.ImagePropertyAlias))
                                                .GetProperty<string>(this.options.ImagePropertyAlias);
                            break;

                        case uQuery.UmbracoObjectType.Media:
                            imageUrl = uQuery.GetMedia(this.CurrentContentId)
                                                .GetAncestorOrSelfMedia()
                                                .First(x => x.HasProperty(this.options.ImagePropertyAlias))
                                                .GetProperty<string>(this.options.ImagePropertyAlias);
                            break;

                        case uQuery.UmbracoObjectType.Member:
                            imageUrl = uQuery.GetMember(this.CurrentContentId).GetProperty<string>(this.options.ImagePropertyAlias);

                            break;
                    }
                }
                catch
                {
                    // node, media or member with specified property couldn't be found
                    // TODO: if debug mode on, then thow exception, else be silent
                }

                if (!string.IsNullOrWhiteSpace(imageUrl))
                {
                    // attempt to get image from the url
                    string imagePath = HttpContext.Current.Server.MapPath(imageUrl);
                    if (File.Exists(imagePath))
                    {                        
                        using (System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath))
                        {
                            this.mainImage.ImageUrl = imageUrl;

                            imageSize = image.Size;
                        }
                    }
                }
            }
            
            if (this.options.Width == 0 && this.options.Height == 0)
            {
                // neither value set, so use image dimensions
                width = imageSize.Width;
                height = imageSize.Height;
            }            
            else if (this.options.Width > 0 && this.options.Height == 0)
            {
                // width set, so calulate height
                width = this.options.Width;
                height = (int)(imageSize.Height / ((decimal)(imageSize.Width / this.options.Width)));
            }            
            else if (this.options.Width == 0 && this.options.Height > 0)
            {
                // height set, so calculate width
                width = (int)(imageSize.Width / ((decimal)(imageSize.Height / this.options.Height)));
                height = this.options.Height;
            }

            // width and height set, so stretch image to fit
            else if (this.options.Width > 0 && this.options.Height > 0)
            {
                width = this.options.Width;
                height = this.options.Height;
            }

            this.mainImage.Width = width;
            this.mainImage.Attributes["width"] = width.ToString();

            this.mainImage.Height = height;
            this.mainImage.Attributes["height"] = height.ToString();
        }
    }
}