using FoxSec.DomainModel.DomainObjects;
using System;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface IVideoCameraService
    {
        string SaveUpdateCameraDetails(string Name, string ServerNr, string CameraNr, string Port, string ResX, string ResY, string Skip, string Delay, string QuickPreviewSeconds, string EnableLiveControls, int? Id, int? type);
    }
}
