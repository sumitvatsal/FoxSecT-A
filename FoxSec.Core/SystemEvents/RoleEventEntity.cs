using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AutoMapper;
using FoxSec.Common.Extensions;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Core.SystemEvents
{
	public class RoleEventEntity : ILogEventEntity 
	{
		public RoleEventEntity(Role role)
		{
			OldValue = new RoleEntity();
			NewValue = new RoleEntity();
			Mapper.Map(role, OldValue);
			OldValue.Menues = new byte[400];
			int i = 0;
			foreach (var menu in role.Menues)
			{
				OldValue.Menues[i] = menu;
				i++;
			}
			i = 0;
			OldValue.Permissions = new byte[400];
			foreach (var perm in role.Permissions)
			{
				OldValue.Permissions[i] = perm;
				i++;
			}
		}

		public void SetNewRole(Role role)
		{
			Mapper.Map(role, NewValue);
		}
		
		public RoleEntity OldValue { get; set; }

		public RoleEntity NewValue { get; set; }

		public string GetCreateMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageRoleCreated", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDescription", new List<string> { OldValue.Description }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageRoleType", new List<string> { OldValue.RoleType }));

			foreach (var roleBuilding in OldValue.RoleBuildings)
			{
				if( roleBuilding.IsDeleted == false )
				{
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuilding", new List<string> { roleBuilding.BuildingName }));
				}
			}
			
			return message.ToString();

		}

		public string GetDeleteMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageRoleDeleted", new List<string> { OldValue.Name }));
			return message.ToString();
		}

		public string GetEditMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageRoleChanged", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNameChanged", new List<string> { OldValue.Name, NewValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDescriptionChanged", new List<string> { OldValue.Description, NewValue.Description }));

			foreach (var roleBuilding in NewValue.RoleBuildings)
			{
				if (roleBuilding.IsDeleted == false)
				{
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuilding", new List<string> { roleBuilding.BuildingName }));
				}
			}
			
            foreach (var menu in Enum.GetValues(typeof(Menu)))
            {
                var isAllowed_old = OldValue.Menues[(int)menu] != 0 ? "true" : "false";

                var isAllowed_new = NewValue.Menues[(int)menu] != 0 ? "true" : "false";

				message.Add(XMLLogMessageHelper.TemplateToXml(Enum.GetName(typeof(Menu), menu), null));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageAllowedIsChanged", new List<string> { isAllowed_old, isAllowed_new }));
			}

			foreach (var perm in Enum.GetValues(typeof(Permission)))
			{
				var isAllowed_old = OldValue.Permissions[(int)perm] != 0 ? "true" : "false";

				var isAllowed_new = NewValue.Permissions[(int)perm] != 0 ? "true" : "false";

				message.Add(XMLLogMessageHelper.TemplateToXml(Enum.GetName(typeof(Menu), perm), null));
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageAllowedIsChanged", new List<string> { isAllowed_old, isAllowed_new }));
			}
			
			return message.ToString();
		}
	}
}
