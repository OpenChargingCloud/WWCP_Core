# WWCP Core

The *World Wide Charging Protocol Suite* is a collection of protocols in order to
connect market actors in the field of e-mobility solutions via *scalable and secure
Internet protocols*. This repository defines the fundamental core concepts, entities
and data structures and comes with a reference implementation of virtual EVSEs, charging
stations, charging pools, charging station operators, e-mobility providers, roaming networks
and more, to enable continous integration tests within e-mobility roaming networks.

#### Related Projects

Serveral project make use of this core library:
 - [WWCP OICP](https://github.com/OpenChargingCloud/WWCP_OICP) defines a mapping between WWCP and the [Open InterCharge Protocol](http://www.intercharge.eu) as defined by [Hubject GmbH](http://www.hubject.com) and thus allows you to enable EMP and CPO roaming.
 - [WWCP OCHP](https://github.com/OpenChargingCloud/WWCP_OCHP) defines a mapping between WWCP and the [Open Clearing House Protocol](http://www.ochp.eu) and thus allows you to enable EMP and CPO roaming.
 - [WWCP OIOI](https://github.com/OpenChargingCloud/WWCP_OIOI) defines a mapping between WWCP and the [OIOI Protocol](https://docs.plugsurfing.com) as defined by [PlugSurfing](https://www.plugsurfing.com) and thus allows you to enable EMP and CPO roaming.
 - [WWCP eMIP](https://github.com/OpenChargingCloud/WWCP_eMIP) defines a mapping between WWCP and the [eMobility Protocol Inter-Operation](https://www.gireve.com/wp-content/uploads/2017/02/Gireve_Tech_eMIP-V0.7.4_ProtocolDescription_1.0.2_en.pdf) as defined by [Gireve](https://www.gireve.com) and thus allows you to enable EMP and CPO roaming.
 - [WWCP OCPI](https://github.com/OpenChargingCloud/WWCP_OCPI) defines a mapping between WWCP and the [Open Charge Point Interface](https://github.com/ocpi/ocpi) and thus allows you to enable EMP and CPO roaming via direct connections betwewen those entities.
 - [WWCP OCPP](https://github.com/OpenChargingCloud/WWCP_OCPP) defines a mapping between WWCP and the [Open Charge Point Protocol](http://www.openchargealliance.org) and thus allows you to attach OCPP charging stations.

The following project are compatible solutions:
- [ChargySharp](https://github.com/OpenChargingCloud/ChargySharp) The C# reference implementation of the Chargy e-mobility transparency software.
- [Chargy Desktop App](https://github.com/OpenChargingCloud/ChargyDesktopApp) An e-mobility transparency software for Windows, Mac OS X and Linux (based on the [Electron framework](https://electronjs.org/)).
- [Chargy Mobile App](https://github.com/OpenChargingCloud/ChargyMobilepApp) An e-mobility transparency software for iOS and Android (based on [Apache Cordova](https://cordova.apache.org)).

#### Requirements & Configuration

1. You will need .NET6
2. (Stress) tested under Debian GNU/Linux running in a KVM environment on AMD Ryzen 9 16-Core Zen3 machines


#### Your contributions

This software is developed by [GraphDefined GmbH](http://www.graphdefined.com).
We appreciate your participation in this ongoing project, and your help to improve it.
If you find bugs, want to request a feature or send us a pull request, feel free to
use the normal GitHub features to do so. For this please read the
[Contributor License Agreement](Contributor%20License%20Agreement.txt)
carefully and send us a signed copy.
