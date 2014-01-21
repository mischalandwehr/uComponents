using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.PropertyEditors;

namespace uComponents.PropertyEditors
{
	using uComponents.Core;

	public class LegacyDataTypeMapping : ApplicationEventHandler
	{
		protected override void ApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			base.ApplicationInitialized(umbracoApplication, applicationContext);

			var NO_MAPPING = Umbraco.Core.Constants.PropertyEditors.NoEditAlias;
			var NO_MAPPING_YET = NO_MAPPING; // [LK] Using this variable to mark the property-editors that we know are in development

			var mappings = new Dictionary<string, string>
			{
				{ Constants.DataTypes.AutoCompleteId, NO_MAPPING },
				{ Constants.DataTypes.CharLimitId, NO_MAPPING_YET },
				{ Constants.DataTypes.CheckBoxTreeId, NO_MAPPING },
				{ Constants.DataTypes.CountryPickerId, NO_MAPPING },
				{ Constants.DataTypes.DataTypeGridId, NO_MAPPING },
				{ Constants.DataTypes.DropdownCheckListId, NO_MAPPING },
				{ Constants.DataTypes.ElasticTextBoxId, NO_MAPPING },
				{ Constants.DataTypes.EnumCheckBoxListId, NO_MAPPING },
				{ Constants.DataTypes.EnumDropDownListId, NO_MAPPING },
				{ Constants.DataTypes.FileDropDownListId, NO_MAPPING },
				{ Constants.DataTypes.FilePickerId, NO_MAPPING_YET },
				{ Constants.DataTypes.ImageDropdownId, NO_MAPPING },
				{ Constants.DataTypes.ImagePointId, NO_MAPPING },
				{ Constants.DataTypes.IncrementalTextBoxId, NO_MAPPING },
				{ Constants.DataTypes.JsonDropdownId, NO_MAPPING },
				{ Constants.DataTypes.MultiNodeTreePickerId, Umbraco.Core.Constants.PropertyEditors.MultiNodeTreePickerAlias },
				{ Constants.DataTypes.MultiPickerRelationsId, NO_MAPPING },
				{ Constants.DataTypes.MultipleDatesId, NO_MAPPING },
				{ Constants.DataTypes.MultipleTextstringId, NO_MAPPING },
				{ Constants.DataTypes.MultiUrlPickerId, NO_MAPPING },
				{ Constants.DataTypes.NotesId, NO_MAPPING_YET },
				{ Constants.DataTypes.PropertyPickerId, NO_MAPPING },
				{ Constants.DataTypes.RadioButtonListWithImagesId, NO_MAPPING },
				{ Constants.DataTypes.RelatedLinksWithMediaId, NO_MAPPING },
				{ Constants.DataTypes.RelationLinks, Umbraco.Core.Constants.PropertyEditors.RelatedLinksAlias },
				{ Constants.DataTypes.RenderMacroId, NO_MAPPING },
				{ Constants.DataTypes.SimilarityId, NO_MAPPING },
				{ Constants.DataTypes.SliderId, Umbraco.Core.Constants.PropertyEditors.SliderAlias },
				{ Constants.DataTypes.SqlAutoCompleteId, NO_MAPPING },
				{ Constants.DataTypes.SqlCheckBoxListId, NO_MAPPING },
				{ Constants.DataTypes.SqlDropDownListId, NO_MAPPING },
				{ Constants.DataTypes.StyledTextBoxId, NO_MAPPING },
				{ Constants.DataTypes.SubTabs, NO_MAPPING },
				{ Constants.DataTypes.TextImageId, NO_MAPPING },
				{ Constants.DataTypes.TextstringArrayId, NO_MAPPING },
				{ Constants.DataTypes.ToggleBoxId, NO_MAPPING },
				{ Constants.DataTypes.UniquePropertyId, NO_MAPPING },
				{ Constants.DataTypes.UrlPickerId, NO_MAPPING },
				{ Constants.DataTypes.UserPickerId, NO_MAPPING },
				{ Constants.DataTypes.XmlDropDownListId, NO_MAPPING },
				{ Constants.DataTypes.XPathAutoCompleteId, NO_MAPPING },
				{ Constants.DataTypes.XPathCheckBoxListId, Umbraco.Core.Constants.PropertyEditors.XPathCheckBoxListAlias },
				{ Constants.DataTypes.XPathChildNodePickerId, NO_MAPPING },
				{ Constants.DataTypes.XPathDropDownListId, Umbraco.Core.Constants.PropertyEditors.XPathDropDownListAlias },
				{ Constants.DataTypes.XPathRadioButtonListId, NO_MAPPING },
				{ Constants.DataTypes.XPathTemplatableListId, NO_MAPPING }
			};

			mappings.All(x => LegacyPropertyEditorIdToAliasConverter.CreateMap(Guid.Parse(x.Key), x.Value));
		}
	}
}