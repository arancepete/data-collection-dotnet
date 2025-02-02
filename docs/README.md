# Data Collection for .NET documentation

<!-- MDTOC maxdepth:6 firsth1:0 numbering:0 flatten:0 bullets:1 updateOnSave:1 -->

- [Description](#description)   
   - [Generic application](#generic-application)   
   - [Trees of Portland](#trees-of-portland)   
   - [Custom behavior](#custom-behavior)   
- [Using the app](#using-the-app)   
   - [Sign in and out of ArcGIS](#sign-in-and-out-of-arcgis)   
   - [App work mode](#app-work-mode)   
      - [_Online Work Mode_](#_online-work-mode_)   
      - [_Offline Work Mode_](#_offline-work-mode_)   
   - [View map bookmarks](#view-map-bookmarks)   
   - [View the map's legend](#view-the-maps-legend)   
   - [Hide and show layers with the TOC](#hide-and-show-layers-with-the-toc)   
   - [Identify map features](#identify-map-features)   
   - [Add map feature](#add-map-feature)   
      - [View a pop-up](#view-a-pop-up)   
      - [Edit a pop-up](#edit-a-pop-up)   
         - [_Editing a Pop-up's Related Records_](#_editing-a-pop-ups-related-records_)   
- [Using web maps](#using-web-maps)   
   - [Configure web map & feature services for data collection](#configure-web-map-feature-services-for-data-collection)   
      - [Map title](#map-title)   
      - [Organizing feature layers](#organizing-feature-layers)   
      - [Feature layer visibility range](#feature-layer-visibility-range)   
      - [Enable editing on feature layers and tables](#enable-editing-on-feature-layers-and-tables)   
      - [Enable pop-up on feature layers and tables](#enable-pop-up-on-feature-layers-and-tables)   
      - [Configure pop-up on feature layers and tables](#configure-pop-up-on-feature-layers-and-tables)   
- [Identity model](#identity-model)   
   - [Public map, social login](#public-map-social-login)   
- [Using map definition & pop-up configurations to drive app behavior](#using-map-definition-pop-up-configurations-to-drive-app-behavior)   
   - [Multiple identify results](#multiple-identify-results)
   - [Add feature rules](#add-feature-rules)   
   - [Pop-up view rules](#pop-up-view-rules)   
      - [View mode](#view-mode)   
         - [_Pop-up Attributes_](#_pop-up-attributes_)   
         - [_Many-To-One Records_](#_many-to-one-records_)   
         - [_One-To-Many Records_](#_one-to-many-records_)   
         - [_Delete Pop-up_](#_delete-pop-up_)   
      - [Edit mode](#edit-mode)   
         - [Pop-up Attributes](#pop-up-attributes)   
         - [_Many-To-One Related Records_](#_many-to-one-related-records_)   
- [Leveraging ArcGIS](#leveraging-arcgis)   
   - [Identifying map features](#identifying-map-features)   
   - [Offline map jobs](#offline-map-jobs)   
      - [Download map offline](#download-map-offline)   
      - [Synchronize offline map](#synchronize-offline-map)   
      - [Deleting offline map](#deleting-offline-map)   
   - [Querying feature tables](#querying-feature-tables)   
      - [Query for all features](#query-for-all-features)   
      - [Query for related features](#query-for-related-features)   
      - [Spatial query](#spatial-query)   
   - [Editing features](#editing-features)   
      - [Creating features](#creating-features)   
      - [Editing features and related records](#editing-features-and-related-records)   
   - [Reverse geocoding](#reverse-geocoding)   
- [Architecture](#architecture)   
   - [Solution overview](#solution-overview)   
   - [Model-View-ViewModel pattern](#model-view-viewmodel-pattern)   
   - [General application organization](#general-application-organization)   
   - [App modes](#app-modes)   
   - [ViewModels organization](#viewmodels-organization)   
   - [Internationalization](#internationalization)   
   - [Automated tests](#automated-tests)
- [Configuration and customization](#configuration-and-customization)   
   - [App static configuration](#app-static-configuration)   
   - [App dynamic configuration](#app-dynamic-configuration)   

<!-- /MDTOC -->
---

## Description

This is a data collection app that uses your organization's web maps and the ArcGIS Web GIS information model. Use the example _Trees of Portland_ web map and dataset to get started.

### Generic application

The app was designed to work in a generic context and thus your organization can configure the app to consume your own web map, out of the box. To accomplish this, the web map is configured by a set of rules that the app adheres to, driving the app's behavior. These rules are defined by the map's definition and by the map's layers' pop-up configurations. To learn more about what drives the app's behavior, read the section entitled [_Using Map Definition & Pop-up Configurations to Drive App Behavior_](#using-map-definition--pop-up-configurations-to-drive-app-behavior).

Theming of the app's UI is controlled by values defined in files in the **Themes** folder. Each platform has light and dark variants. For more significant changes to the appearance of controls, see the **Styles** folder.

### Trees of Portland

The capabilities of the app can be demonstrated using _Trees of Portland_, a web map hosted and maintained by the Esri Runtime organization that ships with the app by default. _Trees of Portland_ tells the story of a city arborist or engaged citizen who maintains inspections for all street trees in the city of Portland, OR.

Users can identify existing or create new street trees of a certain species on a map. Street trees are symbolized on the map based on their condition. Users can collect or view inspection records of those trees over time. The map also contains a symbolized neighborhoods layer to help distribute inspection regions.

The _Trees of Portland_ dataset schema is simple.

![Architecture diagram showing the Trees of Portland database schema](/docs/images/general-ui.png)

A street tree can be one of many species and a street tree can contain zero to many inspection records. A neighborhood is a spatial feature symbolized on the map that does not relate to other tables.

> The neighborhood layer is not related to the other layers and provides the map with a visual context through the use of symbology. The neighborhood layer is queried to populate an attribute of the tree layer.

### Custom behavior

There are a select few custom behaviors displayed in this example application that help tell the _Trees of Portland_ story that won't fit a generic context. The app only performs these custom behaviors if the current map's portal item id matches that of the _Trees of Portland_ web map. In the event a different map is configured, these custom behaviors are ignored. These custom behaviors accomplish the following:

* Upon the creation of a new street tree feature, the app reverse geocodes the tree's location for an address to populate the tree feature's attributes.
* Upon the creation of a new street tree feature, the app queries the neighborhood feature layer for features where the new tree's location falls within the neighborhood's polygon.
* A third customization addresses a current limitation in the platform. As noted earlier, the symbology in the web map reflects a tree's last reported condition. Representing symbology based on a related record is not yet a supported at the web map level. In this app, custom logic is applied whenever a tree inspection is updated or added. All inspections for the given tree are sorted in descending order by inspection date. The condition and DBH (diameter at breast height) of the most recent inspection are used to update the corresponding fields in the tree feature table. In this way, the symbology in the web map reflects the latest inspection.

While these custom behaviors may not work with your web map, they illustrate best practices for using the ArcGIS Runtime SDK. You can remove this custom behavior logic altogether, if you prefer.

## Using the app

The app launches a map view containing the pre-configured web map.

| UWP | WPF |
|-----|-----|
| ![Main Map View](/docs/images/anatomy-map-view-uwp.png) | ![Main Map View](/docs/images/anatomy-map-view.png) |

### Sign in and out of ArcGIS

Upon first launch, the user is not authenticated and the app does not prompt for authentication. To sign in, the user can tap the 'Sign in' button in the title bar.

Upon successfully signing in, the button that previously read 'Sign in' is replaced by the user's name and picture. Clicking the user's name will reveal a menu with the option to sign out.

### App work mode

The app supports a workflow for users in the field with the requirement to work both in connected (online) and disconnected (offline) environments.

#### _Online Work Mode_

At initial launch the app loads the configured portal's public web map. A user does not need to authenticate to use the app provided that the web map and all of its layers are not private. The map can identify features and make edits. Edits can be made to the web map including adding new, updating existing and deleting records.

#### _Offline Work Mode_

A user may need to collect data in a location where they are disconnected from the network. The app allows the user to take a web map offline. Because Trees of Portland uses a premium content basemap, a user must be authenticated to fully take the web map offline.

| UWP | WPF |
|-----|-----|
| ![Download Map Offline Extent](/docs/images/anatomy-offline-extent-uwp.png) | ![Download Map Offline Extent](/docs/images/anatomy-offline-extent.png) |

When taking the web map offline, the app asks the user to specify the area of the web map they want to take offline. The app makes use of the [offline map](https://developers.arcgis.com/net/offline-maps-scenes-and-data/) creation on-demand workflow. After the generate offline map job finishes, the app enters offline work mode and loads the offline mobile map package.

> If you perform this behavior using *Trees of Portland* you should expect the download job to take 10 minutes or so to complete.

Edits made to the offline mobile map's geodatabase remain offline until the user returns to a network connected environment where then they can bi-directionally synchronize changes made to the offline geodatabase with those made to the online web map.

If a user elects to delete the offline map, the app deletes the offline mobile map package from the device and switches to online work mode.

> A user can resume work online without deleting the offline map.

### View map bookmarks

Web maps can include a list of bookmarks. Each bookmark consists of a map extent (visible area) and a name. Bookmarks can be authored in ArcGIS Pro and the ArcGIS Web Map Viewer.

You can click the Bookmark icon to see a list of bookmarks in the map. Selecting a bookmark will show that bookmark's extent.

| UWP | WPF |
|-----|-----|
| ![Screenshot showing bookmarks panel on UWP](/docs/images/component-bookmarks-uwp.png) | ![Screenshot showing bookmarks panel on WPF](/docs/images/component-bookmarks.png) |

### View the map's legend

You can click the legend icon to view the symbology for each layer.

| UWP | WPF |
|-----|-----|
| ![Screenshot showing legend panel on UWP](/docs/images/component-legend-uwp.png) | ![Screenshot showing legend panel on WPF](/docs/images/component-legend.png) |

### Hide and show layers with the TOC

You can click the layer list icon to see a list of each layer in the web map. You can use the checkboxes to hide or show each layer.

| UWP | WPF |
|-----|-----|
| ![Screenshot showing TOC panel on UWP](/docs/images/component-toc-uwp.png) | ![Screenshot showing TOC panel on WPF](/docs/images/component-toc.png) |

### Identify map features

Tapping or clicking the map performs an identify function on the map. The closest result to the clicked location is chosen, the feature is selected on the map, and a pop-up view is revealed containing the feature's attributes and information from any related tables.

| UWP | WPF |
|-----|-----|
| ![Identified Map Feature](/docs/images/anatomy-identified-feature-uwp.png) | ![Identified Map Feature](/docs/images/anatomy-identified-feature.png) |

If a feature has attachments enabled, you can switch to the attachments tab to view them. The name and size of each attachment is displayed.

### Add map feature

If the map contains a spatial feature layer that adheres to the rules specified in the section entitled [_Add Feature Rules_](#add-feature-rules), the add feature button is enabled. Tapping this button begins the process of adding a new record to the map.

| UWP | WPF |
|-----|-----|
| ![Add New Feature](/docs/images/anatomy-new-feature-uwp.png) | ![Add New Feature](/docs/images/anatomy-new-feature.png)

An action banner appears and a pin drops to the center of the map view. The action banner contains a Save and a Cancel button. The pin remains fixed to the center of the map view as the map is panned and zoomed beneath it. If the user taps the Save button, a new feature is created using the fixed map view's center point translated to a spatial coordinate.

#### View a pop-up

A pop-up view allows the user to interrogate the map view's selected feature in greater detail. The table-based view is broken down into a number of sub-components.

| UWP | WPF |
|-----|-----|
| ![View A Pop-up](/docs/images/anatomy-popup-view-uwp.png) | ![View A Pop-up](/docs/images/anatomy-popup-view.png) |

The first section displays each attribute configured for display. Following the display attributes are each many-to-one related records. In the *Trees of Portland* web map the trees table has one many-to-one relationship, the Species table.

Sections that follow represent every one-to-many related records with the header of that section the name of the related table and an `Add New` button, if that table allows adding new features. In the *Trees of Portland* web map the trees table has one one-to-many relationship, the Inspections table, which does allow adding new features.

Related record rows can be tapped and allows the user to interrogate the related record for more information.

Edit and Delete buttons are present at the bottom of the view if the feature is editable.

#### Edit a pop-up

The pop-up's attributes configured as editable can be edited and validated inline within the same pop-up view.

| UWP | WPF |
|-----|-----|
| ![Edit A Pop-up](/docs/images/anatomy-popup-edit-uwp.png) | ![Edit A Pop-up](/docs/images/anatomy-popup-edit.png) |

As values for fields are updated, the app informs the user of invalid changes and why they are invalid. The pop-up won't save if there are invalid fields.

When editing, you can add attachments. On UWP, you can take a picture if the device has a webcam. Both UWP and WPF implementations allow you to browse the file system to add a file attachment.

Edits can be discarded by tapping X button next to the save button. Saving the changes requires every field to pass validation and can be committed by tapping the save button.

##### _Editing a Pop-up's Related Records_

For related records where the pop-up is the child in the related record relationship (a many-to-one related record) the app allows the user to update to which parent record is related. In the *Trees of Portland* web map this means a user can update the tree's species related record.

For related records where the pop-up is the parent in the related record relationship (a one-to-many related record), the user has to save the current pop-up before making edits. In the *Trees of Portland* web map this means a user won't be able to add/update/delete an Inspection before the tree is finished editing.

## Using web maps

You can author your own web maps in [ArcGIS Online and ArcGIS Enterprise](https://enterprise.arcgis.com/en/portal/latest/use/what-is-web-map.htm) or [ArcGIS Desktop](https://desktop.arcgis.com/en/maps/) and share them in your app via your portal; this is the central power of the Web GIS model built into ArcGIS. Building an app which uses a web map allows the cartography and map configuration to be completed in ArcGIS Online and ArcGIS Enterprise rather than in code. This then allows the map to change over time, without any code changes or app updates. Learn more about the benefits of developing with web maps [here](https://developers.arcgis.com/web-map-specification/). Also, learn about authoring web maps in [ArcGIS Online and ArcGIS Portal](https://doc.arcgis.com/en/arcgis-online/create-maps/make-your-first-map.htm) and [ArcGIS Pro](https://pro.arcgis.com/en/pro-app/help/mapping/map-authoring/author-a-basemap.htm).

Loading web maps in code is easy; the app loads a web map from a portal (which may require the user to sign in, see the [_identity model_](#identity-model) section) with the following code:

``` csharp
new Map(new Uri(Settings.Default.AppSettings.WebmapURL))
```

### Configure web map & feature services for data collection

The app's behavior is configuration driven and the following configuration principles should guide you in the configuration of your **own** web map.

> Always remember to save your web map after changes have been performed!

#### Map title

The web map's title becomes the title of the map in the map view's navigation bar. A succinct, descriptive title is recommended because some screen sizes are quite small.

#### Organizing feature layers

The [order](https://doc.arcgis.com/en/arcgis-online/create-maps/organize-layers.htm) of your web map's [feature layers](https://doc.arcgis.com/en/arcgis-online/reference/feature-layers.htm) matter. Layer precedence is assigned to the top-most layer (index 0) first with the next precedence assigned to the next layer beneath, and so on. This is important because only one feature can be identified at a time. When the app performs an identify operation, the layer whose index is nearest 0 and which returns results is the one whose features will be selected.

#### Feature layer visibility range

It is generally recommended to consider the [visibility range](https://doc.arcgis.com/en/arcgis-online/create-maps/set-visibility.htm) of your feature layers. Beyond this general consideration, only visible layers are returned when an identify operation is performed. You'll want to consider which layers to make visible at what scale.

#### Enable editing on feature layers and tables

You'll want to consider whether to enable or disable [editing](https://doc.arcgis.com/en/arcgis-online/manage-data/edit-features.htm) of your feature layers and tables. Specifically, a user is only able to edit features or records on layers whose backing table has editing enabled. This includes related records for features. For instance, if a feature whose backing table does permit editing has a related record backed by a table that does not have editing enabled, that related record layer cannot be edited by the app.

#### Enable pop-up on feature layers and tables

The app relies on pop-up configurations to identify, view, and edit features and records. You'll want to consider whether to enable or disable [pop-ups](https://doc.arcgis.com/en/arcgis-online/create-maps/configure-pop-ups.htm#ESRI_SECTION1_9E13E02AABA74D5DA2DF1A34F7FB3C63) of your feature layers and tables. Only feature layers and tables that are pop-up-enabled can be identified or edited. Please note, you can have a scenario where you've enabled editing on a layer (as described above) but have disabled pop-ups for the same layer and thus a user will not be able to edit this layer.

#### Configure pop-up on feature layers and tables

For all layers with pop-ups enabled, you'll want to consider how that pop-up is [configured](https://doc.arcgis.com/en/arcgis-online/create-maps/configure-pop-ups.htm#ESRI_SECTION1_0505720B006E43C5B14837A353FFF9EC) for display and editing.

**Pop-up Title**

You can configure the pop-up title with a static string or formatted with attributes. The pop-up's title becomes the title of the pop-up containing view controller's navigation bar. A succinct, descriptive title is recommended because some screen sizes are quite small.

**Pop-up Display**

It is recommended to configure your pop-ups such that their content's [display property](https://doc.arcgis.com/en/arcgis-online/get-started/view-pop-ups.htm) is set to **a list of field attributes**. Using this configuration allows you to designate the display order of that table's attributes. This is important because various visual representations of pop-ups in the app are driven by the attributes display order.

> With the Configure Pop-up pane open, under Pop-up Contents the display property provides a drop down list of options, select **a list of field attributes**.

**Pop-up Attributes**

Precedence is assigned to top-most attributes first (index 0) with the next precedence assigned to the subsequent attributes. Individual attributes can be configured as display, edit, both, or neither.

> With the Configure Attributes window open, attributes can be re-ordered using the up and down arrows.

Within the app, a pop-up view can be in display mode or edit mode and attributes configured as such are made available for display or edit.

These attributes' values are accompanied by a title label, which is configured by the attribute's field alias. It is recommended to configure the field alias with a label that is easily understood to represent what is contained by that field.

**Pop-up subtitle and attribute expressions**

Often, it is hard to distinguish features when there are multiple identify results for features with the same pop-up definition. You can define an [Arcade](https://developers.arcgis.com/arcade/) expression that extracts relevant information into a short subtitle or summary. You can define the pop-up's subtitle by storing that Arcade expression in the pop-up's `Attribute Expressions` collection with a title/alias matching the value defined by the `PopupExpressionForSubtitle` setting. By convention, the app knows to check pop-ups for that attribute expression and display it where appropriate.

> **Note**: By default, the web map viewer will add new attribute expressions to the pop-ups display fields. You can manually configure the pop-up to exclude the subtitle expression from the displayed attributes.

## Identity model

The app leverages the ArcGIS [identity](https://developers.arcgis.com/documentation/security-and-authentication/) model to provide access to resources via the named user login pattern. When attempting to access secured resources such as secured web maps, layers, or premium content, the app prompts you for your organization’s portal credentials used to obtain a token. The ArcGIS Runtime SDKs provide a simple-to-use API for dealing with ArcGIS logins.

The process of accessing token secured services with a challenge handler is illustrated in the following diagram.

![ArcGIS Identity Model](/docs/images/identity.png)

1. A request is made to a secured resource.
2. The portal responds with an unauthorized access error.
3. A challenge handler associated with the authentication manager is asked to provide a credential for the portal.
4. An authentication UI presents modally and the user is prompted to enter a user name and password.
5. If the user is successfully authenticated, a credential (token) is included in requests to the secured service.
6. The identity manager stores the credential for this portal and all requests for secured content includes the token in the request.

For an application to use this pattern, follow these [guides](https://developers.arcgis.com/documentation/security-and-authentication/arcgis-identity/) to register your app.

The `AuthenticationManager` is set up when the app starts and a challenge handler is configured. A challenge by the authentication manager occurs when a request is made to access a secured resource for which the authentication manager has no credential.

``` csharp
// Define the server information for ArcGIS Online
var portalServerInfo = new ServerInfo
{
    ServerUri = new Uri(Settings.Default.AppSettings.ArcGISOnlineURL),
    TokenAuthenticationType = TokenAuthenticationType.OAuthAuthorizationCode,
    OAuthClientInfo = new OAuthClientInfo
    {
        ClientId = Settings.Default.AppSettings.AppClientID,
        RedirectUri = new Uri(Settings.Default.AppSettings.RedirectURL)
    },
};

// Register the ArcGIS Online server information with the AuthenticationManager
Security.AuthenticationManager.Current.RegisterServer(portalServerInfo);

// Create a new ChallengeHandler that uses a method in this class to challenge for credentials
Security.AuthenticationManager.Current.ChallengeHandler = new ChallengeHandler(CreateCredentialAsync);
```

Any time a secured service issues an authentication challenge, the `AuthenticationManager` and the app's `LoginWindowViewModel` work together to broker the authentication transaction. For WPF, the `LoginWindowViewModel` contains the OAuth authorization handler to show a sign-in UI to the user in a web browser control.

```csharp
public Task<IDictionary<string, string>> AuthorizeAsync(Uri serviceUri, Uri authorizeUri, Uri callbackUri)
{
    if (_tcs?.Task.IsCompleted == false)
        throw new Exception("Task in progress");

    _tcs = new TaskCompletionSource<IDictionary<string, string>>();

    // Store the authorization and redirect URLs
    WebAddress = authorizeUri;
    _callbackUrl = callbackUri.AbsoluteUri;

    // Return the task associated with the TaskCompletionSource
    return _tcs.Task;
}
```

When the user successfully authenticates, a URI is passed from the web browser control. The URI is decoded and passed back to the `AuthenticationManager` to retrieve the token. The .NET app retrieves all the necessary information (`AppClientID` and `RedirectURL`) to set up the `AuthenticationManager` from the [Configuration](#configuration-and-customization) file.

Note the value for `RedirectURL`. Combined with the text `auth` to make `data-collection://auth`, this is the [redirect URI](https://developers.arcgis.com/documentation/security-and-authentication/oauth-2.0/serverless-native-apps/) that you configured when you registered your app on your [developer dashboard](https://developers.arcgis.com/applications/). For more details on the user authorization flow, see the [Authorize REST API](https://developers.arcgis.com/rest/users-groups-and-items/authorize.htm).

For more details on configuring the app for OAuth, see [the main README.md](https://github.com/esri/data-collection-dotnet).

### Public map, social login

The app allows a user to authenticate against a portal as well as use social credentials. If a user chooses to authenticate with social credentials and an account is not associated to those credentials, [ArcGIS online](https://doc.arcgis.com/en/arcgis-online/reference/sign-in.htm) creates an account for them.

> There might be additional logic to implement if your portal's web map is configured differently.

## Using map definition & pop-up configurations to drive app behavior

The app operates on a set of rules driven by map definitions and pop-up configurations. To learn how to configure your web map, see the section entitled [_Configure Web Map & Feature Services for Data Collection_](#configure-web-map-feature-services-for-data-collection).

### Multiple identify results

When the user clicks or taps on the map, all of the map's layers and all of the map view's graphics overlays are queried. The results are displayed in a list showing each result's graphical representation, popup title, and configured subtitle.

> **Note**, subtitles are not a standard part of the ArcGIS platform and require special attention to set up.

The subtitle is computed via an [Arcade expression](https://developers.arcgis.com/arcade/) defined in the pop-up configuration's *Attribute Expressions* collection. The app will only display a subtitle if the attribute expression has the alias or title set to match the **PopupExpressionForSubtitle** key defined in the app's settings.

> **Note**, the key is set to `subtitle` by default. That value is used for the tree survey sample web map.

### Add feature rules

A user can add new spatial features to the map given those feature layers adhere to certain rules. An `ArcGISFeature` can be added to a layer if:

* the layer is editable
* the layer can add a feature
* the layer is a spatial layer of geometry type: point
* the layer has enabled pop-ups

### Pop-up view rules

An `IdentifiedFeaturePopup` view was designed to view and edit a pop-up. The view state can be either view mode or edit mode, each permitting certain user interaction. The logic for the `IdentifiedFeaturePopup` is contained in the `IdentifiedFeatureViewModel`. To learn more about the `IdentifiedFeatureViewModel`, see the section entitled [_Editing Features_](#editing-features).

#### View mode

The title of the pop-up view reflects the title of the pop-up as configured in portal. The `IdentifiedFeaturePopup` view is tabled-based and populates itself with attribute and related record content in the following ways.

| UWP | WPF |
|-----|-----|
| ![Pop-up View Anatomy Relationships](/docs/images/anatomy-popup-view-relationships-uwp.png) | ![Pop-up View Anatomy Relationships](/docs/images/anatomy-popup-view-relationships.png) |

##### _Pop-up Attributes_

The first section *(highlighted in purple)* is the Attributes section. A `ListView` is bound to the `DisplayFields` property of the `PopupManager` and a `DataTemplate` is set up to display the `Field`'s `Label` and `FormattedValue`.

```xml
<!--List of attributes from identified feature-->
<ListView ItemsSource="{Binding PopupManager.DisplayedFields}" >
    <ListView.ItemTemplate>
        <DataTemplate>
            <StackPanel>
                <TextBlock Text="{Binding Field.Label}" FontWeight="Bold" />
                <TextBox IsEnabled="false" Text="{Binding FormattedValue, Mode=OneWay}" />
            </StackPanel>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

##### _Many-To-One Records_

Following the attributes, a second `ListView` represents each many-to-one related record *(highlighted in blue)* associated with the pop-up. The row displays the first two display attributes of the related record.

A user can tap the related record row (indicated by a green button with an arrow). Doing so reveals a new `DestinationRelatedRecordPopup` containing the related record.

##### _One-To-Many Records_

The subsequent section *(highlighted in orange)* represents a collection of one-to-many related records, one section for every one-to-many related record type. The header label for that section reflects the table name of the related record's feature table. If the section's table permits adding new features, there will be a plus sign in the header next to the table name. Every subsequent row represents a single one-to-many record and displays the first three display attributes of the related record.

A user can tap the related record row (indicated by a green button with an arrow). Doing so reveals a new `OriginRelatedRecordPopup` containing the related record. A user can also swipe the table cell to the left revealing update and delete actions for the related record, if the containing feature table permits it.

##### _Delete Pop-up_

The footer bar contains buttons to edit or delete the feature. These buttons are revealed only if the table permits edit and delete.

#### Edit mode

Starting an editing session requires that the `PopupManager` allows editing.

##### Pop-up Attributes

Every field determined by the manager's `EditableDisplayFields` is represented by its own row inside a `ListView`. The `ListView` is bound to the `Fields` property of the `PopupManager` and the `DataTemplate` is dynamically created based on the field's data type using a `DataTemplateSelector`.

```csharp
public override DataTemplate SelectTemplate(object item, DependencyObject container)
{
    var element = container as FrameworkElement;

    if (element != null && item != null && item is FieldContainer popupFieldValue)
    {
        if (popupFieldValue.OriginalField.FieldType == FieldType.Date)
        {
            return element.FindResource("DateTemplate") as DataTemplate;
        }
        else if (popupFieldValue.OriginalField.Domain != null && popupFieldValue.OriginalField.Domain is CodedValueDomain)
        {
            return element.FindResource("CodedValueDomainTemplate") as DataTemplate;
        }
        // ...
        else
        {
            return element.FindResource("StringTemplate") as DataTemplate;
        }
    }
    return null;
}
```

Each data template resource can be found in the `ResourceDictionary` file. Below is an example of how the `DateTemplate` is configured:

```xml
<DataTemplate x:Name="DateTemplate" x:Key="DateTemplate">
    <StackPanel>
        <TextBlock Text="{Binding PopupFieldValue.Field.Label}" FontWeight="Bold" Margin="10, 0, 0, 0" />
        <DatePicker Text="{Binding PopupFieldValue.Value, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" Width="230" Margin="10, 0, 0, 0"/>
        <TextBlock Text="{Binding PopupFieldValue.ValidationError.Message}" Visibility="{Binding PopupFieldValue.ValidationError, Converter={StaticResource NullToVisibilityConverter}}" Foreground="Red" TextWrapping="Wrap" Margin="10,0,0,0"/>
    </StackPanel>
</DataTemplate>
```

##### _Many-To-One Related Records_

Editing a many-to-one related record is permitted and follows certain rules. To edit a many-to-one related record, the user can tap or click the arrow in the related record row. The app runs a query for all of the relationship's related records and presents the options in a `ComboBox`. Selecting a new related record stages that record to be saved, should the user save their changes.

If the related record has not been selected and the many-to-one relationship is composite, the pop-up will not validate. Conversely, if the relationship is not composite, the related record can be kept empty. If the related record has been selected, the app does allow the user to change the related record to a different one. Note, the combo box does not allow a user to deselect a many-to-one related record once it has been selected.

## Leveraging ArcGIS

The app demonstrates best practices for consuming the ArcGIS Runtime .NET SDK.

### Identifying map features

The identify tasks are handled inside the app's `IdentifyController`. A new instance of the `IdentifyController` is created in the `MainViewModel`'s constructor and an event listener is responsible for returning to the `MainViewModel` the correctly identified feature when the user taps on the map.

```csharp
public MainViewModel()
{
    // ...
    // Initialize the identify controller
    IdentifyController = new IdentifyController();
    IdentifyController.IdentifyCompleted += IdentifyController_IdentifyCompleted;
}
```

> The `IdentifyController` is generic and allows identifying multiple features and graphics on layers and graphics overlays. The limitations required by the app are applied to the returned features inside the `IdentifyController_IdentifyCompleted` event handler:

```csharp
// get the first result that meets the app rules
var identifyLayerResult = e.LayerResults.Where(l => (l.LayerContent as Layer).IsIdentifiable()).FirstOrDefault();
```

### Offline map jobs

The SDK facilitates taking a web map offline and synchronizing changes through the use of offline tasks.

#### Download map offline

Before generating an offline map job, the app asks the user to establish the area of the web map that they wish to take offline. Once established, the app sets up a `GenerateOfflineMapJob` and sets up parameters.

```csharp
private async Task DownloadPackageAsync(Envelope extent)
{
    // ...
    // set extent based on screen
    var parameters = await syncTask.CreateDefaultGenerateOfflineMapParametersAsync(extent);

    // retrieve only records related to the features being taken offline
    parameters.DestinationTableRowFilter = DestinationTableRowFilter.RelatedOnly;

    // set the job to generate the offline map
    GenerateOfflineMapJob = syncTask.GenerateOfflineMap(parameters, DownloadPath);
    // ...
}
```

The app passes the extent from the `MapView` to the `DownloadView` through control property binding when setting up the `DownloadView`.

```xml
<views:DownloadView  DataContext="{StaticResource MainViewModel}" VisibleArea="{Binding ElementName=MapView, Path=VisibleArea}" />
```

The first time the user attempts to download an offline map, the app requests from them a download path, which is then stored in the app's config file. Upon a successful download, the app switches to offline mode and displays the map from the newly downloaded mobile map package.

#### Synchronize offline map

Synchronizing a map is even more straightforward than downloading a map. The app builds an `OfflineMapSyncJob` by constructing an offline map sync task using the offline map and specifying the offline sync parameters sync direction `SyncDirection.Bidirectional`.

> A bi-directional sync synchronizes local changes with the web map and changes made to the web map are synchronized with the offline map. Synchronization conflicts are resolved following the rule "last-in wins".

```csharp
private async Task SyncMap(Map map)
{
    var syncTask = await OfflineMapSyncTask.CreateAsync(map);

    // set parameters for sync
    var taskParams = new OfflineMapSyncParameters()
    {
        SyncDirection = SyncDirection.Bidirectional,
        RollbackOnFailure = true
    };

    // set the job to perform the sync
    OfflineMapSyncJob = syncTask.SyncOfflineMap(taskParams);
    // ...

    // begin sync job
    OfflineMapSyncJob.Start();
}
```

#### Deleting offline map

Deleting an offline map is currently not handled by the Runtime API and is performed within the app in two steps. The `MobileMapPackageExtensions` `Close()` method first closes the mobile map package by closing all connections established with the geodatabase. The `ClearDirectory()` method in `IOExtensions` attempts to remove the directory containing the mmpk.

```csharp
foreach (var map in mmpk.Maps)
{
    foreach (var layer in map.AllLayers.OfType<FeatureLayer>())
    {
        if (layer.FeatureTable is GeodatabaseFeatureTable)
        {
            ((GeodatabaseFeatureTable)layer.FeatureTable).Geodatabase.Close();
        }
    }
    foreach (var table in map.Tables)
    {
        if (table is GeodatabaseFeatureTable)
        {
            ((GeodatabaseFeatureTable)table).Geodatabase.Close();
        }
    }
}
```

> This method only works if the mobile map package is not in use. In order to ensure the mmpk is not in use, the app kicks user into Online Mode when they attempt to delete the offline map.

If the app is unsuccessful in closing and deleting the mmpk, to avoid corrupt data, the app will restart.

### Querying feature tables

There are number of things to note when performing a query on an ArcGIS feature table. There are two concrete subclasses of an ArcGIS feature table. A `ServiceFeatureTable` represents an online web map feature table. Alternatively, a `GeodatabaseFeatureTable` represents an ArcGIS offline mobile map package's geodatabase feature table.

There is one key difference in how the app queries these differing table types for features. By default, a `GeodatabaseFeatureTable` loads all attributes of the records it returns in the query result. Conversely, when using a `ServiceFeatureTable` the app must specify the `QueryFeatureFields` parameter as `QueryFeatureFields.LoadAll` otherwise the server returns a feature without all of its attributes loaded.

The app contains a `FeatureTableExtensions` class with extensions that facilitate querying either service feature table or geodatabase feature table returning all fully loaded results and follow the familiar feature table query pattern.

#### Query for all features

The app queries for all records in a table under the circumstance where a user would like to relate a many-to-one related record. This is accomplished by specifying the `QueryParameters` SQL-like `WhereClause` to `"1 = 1"`.

```csharp
internal static async Task<FeatureQueryResult> QueryFeatures(this FeatureTable featureTable, QueryParameters queryParameters)
{
    if (featureTable is ServiceFeatureTable serviceFeatureTable)
    {
        return await serviceFeatureTable.QueryFeaturesAsync(queryParameters, QueryFeatureFields.LoadAll);
    }
    else if (featureTable is GeodatabaseFeatureTable geodatabaseFeatureTable)
    {
        return await geodatabaseFeatureTable.QueryFeaturesAsync(queryParameters);
    }
    return null;
}
```

#### Query for related features

There are a number of cases where the app queries a feature for its related records. The feature's containing feature table accomplishes this task by providing the feature in question and a relationship information that specifies which related record type to return.

```csharp
internal static async Task<IReadOnlyList<RelatedFeatureQueryResult>> GetRelatedRecords(this FeatureTable featureTable, Feature feature, RelationshipInfo relationshipInfo)
{
    var parameters = new RelatedQueryParameters(relationshipInfo);

    if (featureTable is ServiceFeatureTable serviceFeatureTable)
    {
        return await serviceFeatureTable.QueryRelatedFeaturesAsync((ArcGISFeature)feature, parameters, QueryFeatureFields.LoadAll);
    }
    else if (featureTable is GeodatabaseFeatureTable geodatabaseFeatureTable)
    {
        return await geodatabaseFeatureTable.QueryRelatedFeaturesAsync((ArcGISFeature)feature, parameters);
    }
    return null;
}
```

Once related records have been retrieved, the app creates corresponding viewmodels to handle the viewing and editing of related records.

A `DestinationRelationshipViewModel` is created for every many-to-one relationship returned in the above query. The `DestinationRelationshipViewModel` is responsible for creating a pop-up for the related record in the relationship and for retrieving all of the available related records for when the user choses to edit the many-to-one relationship.

An `OriginRelationshipViewModel` is created for every one-to-many relationship returned in the above query. The `OriginRelationshipViewModel` is responsible for creating pop-ups for all related records involved in the relationship and to handle adding and editing of one-to-many related records.

The pop-ups in the viewmodels are then bound to the views in order to display information about the selected related record.

#### Spatial query

The *Trees of Portland* story contains custom behavior to perform a spatial query on the neighborhoods layer to obtain a neighborhood's name which is populated into a tree's attributes. This spatial query is specified by a `QueryParameters` object. In our example, we query the neighborhoods table for a neighborhood where the new tree's point falls within the bounds of the neighborhood's polygon.

```csharp
internal static async Task<string> GetNeighborhoodForAddedFeature(FeatureTable neighborhoodsTable, MapPoint newTreePoint)
{
    // set the parameters for the query
    // we want only one neighborhood that intersects the geometry of the newly added tree
    var queryParams = new QueryParameters()
    {
        ReturnGeometry = false,
        Geometry = newTreePoint,
        SpatialRelationship = SpatialRelationship.Intersects,
    };

    await neighborhoodsTable.LoadAsync();
    var featureQueryResult = await neighborhoodsTable.QueryFeaturesAsync(queryParams);

    // ...

}
```

### Editing features

The app's base data model object, `Popup`, can be broken down generally into two parts. The first is the pop-up's `GeoElement` which in our case is always an instance of an `ArcGISFeature`. The second is the web map's configuration of that feature as a `Popup`, defined by a `PopupDefinition`.

Editing of a `Popup` is facilitated by the [`PopupManager`](https://developers.arcgis.com/net/latest/wpf/api-reference/html/T_Esri_ArcGISRuntime_Mapping_Popups_PopupManager.htm).

#### Creating features

When creating a new feature, the app must also take the next step and builds a pop-up using the newly-created feature and its feature table's pop-up definition.

```csharp
// create new feature
var feature = ((FeatureLayer)layer).FeatureTable.CreateFeature();

// set feature geometry as the mapview's center
var newFeatureGeometry = MapViewModel.AreaOfInterest?.TargetGeometry.Extent.GetCenter() as MapPoint;

 // create the feature and its corresponding viewmodel
IdentifiedFeatureViewModel = new IdentifiedFeatureViewModel(feature, ((FeatureLayer)layer).FeatureTable, AppState)
{
    EditViewModel = new EditViewModel(AppState)
};
IdentifiedFeatureViewModel.EditViewModel.CreateFeature(newFeatureGeometry, feature as ArcGISFeature, IdentifiedFeatureViewModel.PopupManager);

// select the new feature
MapViewModel.SelectFeature(feature);
```

#### Editing features and related records

The `EditViewModel`, facilitates editing features and related records. Validation inside the `PopupManager` for edited `PopupFieldValue`s is automatically performed on the app through the API's `ValidationError` property. Since the API does not enforce validation, the app provides an extension to `PopupManager` that does a final validation check before allowing user to save the edited feature.

```csharp
internal static bool HasValidationErrors(this PopupManager popupManager)
{
    foreach (var field in popupManager.EditableDisplayFields)
    {
        if (popupManager.GetValidationError(field.Field) != null)
        {
            return true;
        }
    }
    return false;
}
```

Once the user decides to persist changes to the feature, the `EditViewModel` first commits changes to each many-to-one related records before calling its super class function.

```csharp
internal async Task<Feature> SaveEdits(PopupManager popupManager, FeatureTable table, ObservableCollection<DestinationRelationshipViewModel> destinationRelationships)
{
    // feature that was edited
    var editedFeature = popupManager.Popup.GeoElement as ArcGISFeature;

    // ...

    // exit the PopupManager edit session
    await popupManager.FinishEditingAsync();

    // get relationship changes (if they exist) and relate them to the feature
    foreach (var destinationRelationship in destinationRelationships ?? new ObservableCollection<DestinationRelationshipViewModel>())
    {
        if (destinationRelationship.PopupManager != null)
        {
            try
            {
                editedFeature.RelateFeature((ArcGISFeature)destinationRelationship.PopupManager.Popup.GeoElement, destinationRelationship.RelationshipInfo);
            }

            // ...
        }
    }

    // update feature
    await table.UpdateFeatureAsync(editedFeature);

    // ...

}
```

### Reverse geocoding

The *Trees of Portland* story contains a custom behavior that reverse geocodes a point into an address which is populated into a tree's attributes. In order to support both an online and an offline work flow, the app uses the [world geocoder web service](https://developers.arcgis.com/features/geocoding/) to perform reverse geocoding while online, and a custom offline locator that is included in the app's shared `Resources` folder for offline reverse geocoding tasks.

```csharp
// try using the online geocoder
// if that fails, use the side-loaded offline locator
try
{
    locator = await LocatorTask.CreateAsync(new Uri(Settings.Default.AppSettings.GeocodeUrl));
}
catch
{
    locator = await LocatorTask.CreateAsync(
        new Uri(Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).ToString(), OfflineLocatorPath)));
}
```

Because the *Trees of Portland* web map stores the results of a geocode operation, the reverse geocode parameters must have set `IsForStorage = true`. For more on the world geocoding service visit the [developers website](https://developers.arcgis.com/rest/geocode/api-reference/overview-world-geocoding-service.htm).

> The side-loaded geocoder was generated statically whereas the world geocoder service remains current. You might notice a difference in the results between geocoders.

## Architecture

### Solution overview

The Data Collection app is built with cross platform adaptability in mind. The app's code is separated into four projects:

* **DataCollection.Shared** - contains shared code that applies to both platforms.
* **DataCollection.UWP** - UWP UI.
* **DataCollection.WPF_NetFramework** - WPF UI using .NET Framework.
* **DataCollection.WPF_NetCore** - WPF UI using .NET Core.
    * Note: requires Visual Studio 2019 or later

Just as the UWP and WPF projects refer to the shared code project, Xamarin projects can be added to support mobile platforms. There are no substantive differences between the .NET Framework and .NET Core versions of the WPF data collection projects.

### Model-View-ViewModel pattern

The app uses the [MVVM (Model-View-ViewModel)](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) pattern. It makes heavy use of data binding to separate the presentation layer (the view) from the logic of the application (the viewmodel and the model), thus facilitating code sharing. Generally speaking, in .NET, business logic is commonly cross-platform compatible, whereas the presentation layer is often not.

The application is structured to demonstrate separation of concerns using the Model-View-ViewModel (MVVM) architecture. The views have minimal logic in the code behind file and it is all view related. According to MVVM principles, the model should not know about the viewmodel and the viewmodel should not know about the view. The way the viewmodel communicates with the view is through bindable properties. A good example of this is the `Map` property on the `MapView` control.

```csharp
private Map _map;

/// <summary>
/// Gets or sets the map
/// </summary>
public Map Map
{
    get { return _map; }
    set
    {
        _map = value;
        OnPropertyChanged();
    }
}
```

The map is bound to the `Map` property on the `MapView` control in XAML

```xml
<esri:MapView x:Name="MapView" Map="{Binding MapViewModel.Map, Mode=TwoWay, Source={StaticResource MainViewModel}}" />
```

In order to adhere to MVVM principles, properties that are not part of the view and are not bindable, are set through the use of [attached properties](https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/attached-properties-overview). For example, to be able to control the viewpoint for the `MapView`, the API contains a set of async methods that would need to be accessed in code behind. The app provides a `ViewpointController` that handles calling these async methods and exposes a `Viewpoint` dependency property that can be then bound to in xaml.

```csharp
// Invoked when the MapView's ViewPoint value has changed
private void MapView_ViewpointChanged(object sender, EventArgs e)
{
        _isMapViewViewpointChangedEventFiring = true;
        try
        {
            Viewpoint = (sender as MapView)?.GetCurrentViewpoint(ViewpointType.CenterAndScale);
        }
        // if unable to get the viewpoint, don't do anything
        catch { }
        _isMapViewViewpointChangedEventFiring = false;
}
```

```xml
<utils:MapViewExtensions.ViewpointController>
    <utils:ViewpointController Viewpoint="{Binding MapViewModel.AreaOfInterest, Source={StaticResource MainViewModel}, Mode=TwoWay}"/>
</utils:MapViewExtensions.ViewpointController>
```

### General application organization

The app has one main view (`MainWindow` in WPF and `MainPage` in UWP) and several smaller views that are driven by actions in the main view. The `MainViewModel` is the entry point into the app and acts as both a viewmodel and a viewmodel manager for several smaller, limited lifespan viewmodels. See the [_viewModels organization_](#viewModels-organization) section for more details. Communication of changed properties within the app is performed mostly through bindings and the use of the `INotifyPropertyChanged` implementation. However, there is some communication that occurs which is not served well by these methods. The app uses `Messenger` classes to raise and retrieve changed values. One example of such a property is the app's `ConnectivityMode` property which has multiple instances which all need to be kept in sync throughout the app.

### App modes

The `ConnectivityMode` enum describes the two modes in which the app can work. When `Online`, the app accesses the provided webmap and all edits are performed live on the online dataset. The user is also able to perform actions such as download a map offline and sync an existing offline map. The `Offline` mode does not indicate connectivity state, but app state. Meaning, the app could be in `Offline` mode while the device is still connected to the network. In `Offline` mode, the user is working with the downloaded map and all edits are performed locally and have to be synched before they will appear in the online dataset.

### ViewModels organization

The app uses composition to manage viewmodels. The `MainViewModel` is responsible for creating instances of most of the other viewmodels, as needed throughout the lifetime of the app. The `MapViewModel` also has the same lifespan as the app, but `IdentifiedFeatureViewModel`, `OriginRelationshipViewModel` and `DestinationRelationshipViewModel` are only present when the user is actively working with a feature. Its respective related tables, and `DownloadViewModel` and `SyncViewModel` are alive while user is downloading or syncing a map. The `EditViewModel` is also created and dismissed as needed to perform edit operations requested by the feature handling viewmodels.

The benefit to these limited lifespan viewmodels is the ability to manipulate visibility of controls in the view based on the viewmodels' availability. For example, the visibility of the pop-up displaying the identified feature is bound to the existence of an `IdentifiedFeatureViewModel`.

```xml
<!-- Pop-up containing identified feature information -->
<Grid Visibility="{Binding IdentifiedFeatureViewModel, Converter={StaticResource NullToVisibilityConverter}}" >
    <TextBlock Text="{Binding IdentifiedFeatureViewModel.PopupManager.Title}" />
    <Button Visibility="{Binding IdentifiedFeatureViewModel.EditViewModel,
    Converter={StaticResource NullToVisibilityConverter}, ConverterParameter=Inverse}"
    Content="&#xE8bB;" Command="{Binding ClearIdentifiedFeatureCommand}" />
    <views:IdentifiedFeaturePopup Grid.Row="1" DataContext="{Binding}"/>

    <!-- ... -->

</Grid>
```

 The `NullToVisibilityConverter` is used to translate the existence of the viewmodel into a visibility property for the control.

```csharp
object IValueConverter.Convert(object value, Type targetType, object parameter, CustomCultureInfo culture)
{
    // ...

    //if value is null, visibility is collapsed
    return (value == null) ? Visibility.Collapsed : Visibility.Visible;

    // ...
}
```

### Internationalization

All of the buttons, prompts, and other text containing controls in the app are being populated from a `Resources.resx` file. This allows a developer to easily switch the language of the app by providing the equivalent of the resources file translated in their language. The app reads the resources and populates the text accordingly.

> **Note**: UWP uses a `Resources.resw` file, which is updated to match the contents of `Resources.resx` automatically as part of the solution build process.

Code below demonstrates retrieving text from resources when prompting the user to answer whether they are sure they want to discard edits performed:

```csharp
UserPromptMessenger.Instance.RaiseMessageValueChanged(
    Resources.GetString("DiscardEditsConfirmation_Title"),
    Resources.GetString("DiscardEditsConfirmation_Message"),
    false,
    null,
    Resources.GetString("DiscardButton_Content"));
```

### Automated tests

Automated unit tests, enabled by [NUnit](https://nunit.org/), were added with the 1.3.0 release. Unit tests were only added to substantial new code.

Tests are in the following test projects:

* **DataCollection.Shared.Tests** - includes tests exercising functionality in the shared view models
* **Controls\ControlsTest** - includes test that exercise the custom layout panel used by the app

Because this app uses a shared code project, all of the code in the shared project needs to be compilable when included in the .NET Core unit test project. To enable that, mock UI objects (UI objects aren't available in the .NET standard Runtime libraries) are included in the shared test project. Additional changes were made to the shared code project to ensure that tests can run.

## Configuration and customization

### App static configuration

The shared `Configuration.xml` contains a series of static configuration resources. Modify these configurations to suit your needs. They include:

* **WebmapURL**: The url for the webmap that will populate the app. The app can only work with one webmap at a time
* **ArcGISOnlineURL**: Used for OAuth authentication `https://www.arcgis.com/sharing/rest`
* **DefaultZoomScale**: Integer that sets how far the current location button will zoom in when pushed
* **AppClientID**: Used for OAuth authentication
* **RedirectURL**: Used for OAuth authentication
* **PopupExpressionForSubtitle**: The title of the popup expression to be used to compute a subtitle for features. The feature subtitle helps differentiate features when there are multiple identify results.
* **MaxIdentifyResultsPerLayer**: Sets the maximum number of results to return when performing an identify operation.
* **ShowAppVersion**: Controls whether the app's version is shown in the UI.
* **ShowRuntimeVersion**: Controls whether the ArcGIS Runtime version is shown in the UI.
* **ShowLicenseInfo**: Controls whether a link to show license info is shown in the UI.
* **LicenseInfoLink**: Sets the link to navigate to for license info.

Settings used specifically and only with the *Tree Survey* dataset. These should not be modified.

```xml
<TreeDatasetWebmapUrl>https://runtime.maps.arcgis.com/home/item.html?id=fcc7fc65bb96464c9c0986576c119a92</TreeDatasetWebmapUrl>
<GeocodeUrl>https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer</GeocodeUrl>
<OfflineLocatorPath>\Resources\AddressLocator.loc</OfflineLocatorPath>
<NeighborhoodOperationalLayerId>0</NeighborhoodOperationalLayerId>
<NeighborhoodNameField>NAME</NeighborhoodNameField>
<NeighborhoodAttribute>Neighborhood</NeighborhoodAttribute>
<AddressAttribute>Address</AddressAttribute>
<TreeConditionAttribute>Condition</TreeConditionAttribute>
<InspectionConditionAttribute>Condition</InspectionConditionAttribute>
<TreeDBHAttribute>DBH</TreeDBHAttribute>
<InspectionDBHAttribute>DBH</InspectionDBHAttribute>
```

### App dynamic configuration

The following settings are modified by the app and should not be manually edited:

* ConnectivityMode: the application state, whether online or offline
* OAuthRefreshToken: refresh token for the OAuth authentication
* DownloadPath: the download path for the mobile map package
* SyncDate: the date the mobile map package was last downloaded or synched

The first time the app runs, the config file is generated and saved locally in the user's AppData. That path depends on the version of the app that is run:

* WPF (.NET Framework): `C:\Users\<YourUsername>\AppData\Local\ESRI\Data Collection for .NET (WPF)\Settings.xml`
* WPF (.NET Core): `C:\Users\<YourUsername>\AppData\Local\ESRI\DataCollection.WPF\Settings.xml`
* UWP: `C:\Users\<YourUsername>\AppData\Local\Packages\a5fe2542-a748-4b21-8484-d65c71379b3e_5gsmk95twfe48\LocalState\Esri\Data Collection for .NET (UWP)\Settings.xml`

The app does not need to be rebuilt to change the configured web map. Update the `WebmapURL` in the local config file.
