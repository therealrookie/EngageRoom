# EngageRoom

Interface for the Engage Room for Adidas in Paris

## File Structure

| Page | File Name      | Page Description                                                  |
| ---- | -------------- | ----------------------------------------------------------------- |
| 1    | page1          | Home - first page, select language                                |
| 2    | selectlocality | Select locality or meeting                                        |
| 3    | localuse       | Continue to LED, meeting, or camera. Change audio level           |
| 4    | ledcontrol     | Switch LED input / turn on and off LED panel                      |
| 5    | monitorcontrol | Switch monitor input / turn on and off monitor                    |
| 6    | cameracontrol  | Pan around and zoom in/out with camera                            |
| 7    | meeting        | Pan around and zoom in/out with camera. Add/change custom presets |
|      | fire           | Display goes red for fire alarm                                   |
|      | loading        | Loading screen                                                    |
|      | keypadinput    | Page to insert the Key                                            |

## Contract

#### controlPages

| Type    | State          | Event        | Description                                           |
| ------- | -------------- | ------------ | ----------------------------------------------------- |
| Numeric | pageFb         | page         | Get and set the current page                          |
| Numeric | previousPageFb | previousPage | Get the "previous" page to set again after fire-alarm |
| String  | codeInputFb    | codeInput    |                                                       |

#### selectLanguage

| Type    | State       | Event     | Description |
| ------- | ----------- | --------- | ----------- |
| Boolean | isEnglishFb | isEnglish |             |

#### localUse

| Type    | State                     | Event                   | Description |
| ------- | ------------------------- | ----------------------- | ----------- |
| Boolean | volumeChangedBrandMusicFb | volumeChangedBrandMusic |             |
| Boolean | volumeChangedMicVolumeFb  | volumeChangedMicVolume  |             |
| Boolean | volumeChangedMediaLevelFb | volumeChangedMediaLevel |             |
| Numeric | brandMusicVolumeFb        | brandMusicVolume        |             |
| Numeric | micVolumeFb               | micVolume               |             |
| Numeric | mediaLevelFb              | mediaLevel              |             |

#### ledControl

| Type    | State               | Event             | Description |
| ------- | ------------------- | ----------------- | ----------- |
| Boolean | signageFb           | signage           |             |
| Boolean | tvPlayerFb          | tvPlayer          |             |
| Boolean | laptopInputFb       | laptopInput       |             |
| Boolean | ledOnFb             | ledOn             |             |
| Boolean | ledOffFb            | ledOff            |             |
| Boolean | showChannelsPageFb  | showChannelsPage  |             |
| Boolean | channelsAvailableFb | channelsAvailable |             |
| Boolean | videoConFb          | videoCon          |             |
| Boolean | videoConPermittedFb | videoConPermitted |             |
| String  | selectedChannelFb   | selectedChannel   |             |
| ...     | ...                 | ...               |             |
| String  | channel20Fb         | channel20         |             |

#### monitorControl

| Type    | State               | Event             | Description |
| ------- | ------------------- | ----------------- | ----------- |
| Boolean | signageFb           | signage           |             |
| Boolean | tvPlayerFb          | tvPlayer          |             |
| Boolean | laptopInputFb       | laptopInput       |             |
| Boolean | ledOnFb             | ledOn             |             |
| Boolean | ledOffFb            | ledOff            |             |
| Boolean | showChannelsPageFb  | showChannelsPage  |             |
| Boolean | channelsAvailableFb | channelsAvailable |             |
| Boolean | videoConFb          | videoCon          |             |
| Boolean | videoConPermittedFb | videoConPermitted |             |
| String  | selectedChannelFb   | selectedChannel   |             |
| ...     | ...                 | ...               |             |
| String  | channel20Fb         | channel20         |             |

#### cameraControl

| Type    | State       | Event     | Description |
| ------- | ----------- | --------- | ----------- |
| Boolean | preset01Fb  | preset01  |             |
| Boolean | preset02Fb  | preset02  |             |
| Boolean | preset03Fb  | preset03  |             |
| Boolean | zoomPlusFb  | zoomPlus  |             |
| Boolean | zoomMinusFb | zoomMinus |             |
| Boolean | callFb      | call      |             |
| Boolean | setBtnFb    | setBtn    |             |

#### meetingControl

| Type    | State                          | Event                        | Description |
| ------- | ------------------------------ | ---------------------------- | ----------- |
| Boolean | preset01Fb                     | preset01                     |             |
| Boolean | preset02Fb                     | preset02                     |             |
| Boolean | preset03Fb                     | preset03                     |             |
| Boolean | zoomPlusFb                     | zoomPlus                     |             |
| Boolean | zoomMinusFb                    | zoomMinus                    |             |
| Boolean | volumeChangedRoomSoundVolumeFb | volumeChangedRoomSoundVolume |             |
| Numeric | roomSoundVolumeFb              | roomSoundVolume              |             |

## Shell Template

The shell template uses CH5 library and Vanilla Javascript which helps to kick-start your project to build fast, robust, and adaptable web apps with little changes. The project can be deployed on TSW Panels, Android and iOS devices.

### See www.crestron.com/developer for documentation

https://www.crestron.com/developer

## Shell Template

The project have dependencies that require nodejs(https://nodejs.org), together with NPM.

## Installation

### Install all global dependencies

Run `npm install -g @crestron/ch5-utilities-cli` to deploy the project on device.

### Install all local dependencies

Run `npm run install` to install all dependencies for the project.

### Development server

Run `npm run start` for a dev server. Navigate to `http://localhost:3000/`. The app will automatically reload if you change any of the source files.

## How to deploy the project in TSW device

Need to add the hostname or IP address of TSW device in package.json, like below code
`"build:deploy": "ch5-cli deploy -H hostname -t touchscreen dist/shell-template-project.ch5z"`

### Production build

Run `npm run build:prod` to build the project in production mode. The build artifacts will be stored in the `dist/` directory.

### Create archive

Run `npm run build:archive` to create .ch5z file which supports TSW device. The build artifacts will be stored in the `dist/` directory.

### Deploy the project in TSW device

Run `npm run build:deploy` to deploy the project in TSW device.

### Deploy the project in one step

Run `npm run build:onestep` to build, archive and deploy the project in one step.
