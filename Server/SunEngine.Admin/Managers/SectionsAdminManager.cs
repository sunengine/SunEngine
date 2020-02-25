using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Presenters;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.ConfigItemType;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Admin.Managers
{
	public interface ISectionsAdminManager
	{
		Task CreateSectionAsync(Section section);
		Task UpdateSectionAsync(Section section);
		Task DeleteSectionAsync(int sectionId);
	}

	public class SectionsAdminManager : DbService, ISectionsAdminManager
	{
		protected readonly ICheckRoles checkRoles;
		protected readonly SectionTypes sectionTypes;

		public SectionsAdminManager(
			ICheckRoles checkRoles,
			SectionTypes sectionTypes,
			DataBaseConnection db) : base(db)
		{
			this.sectionTypes = sectionTypes;
			this.checkRoles = checkRoles;
		}

		public Task CreateSectionAsync(Section section)
		{
			SectionsUtils.SectionProcess(section, sectionTypes, checkRoles);
			return db.InsertWithIdentityAsync(section);
		}

		public Task UpdateSectionAsync(Section section)
		{
			SectionsUtils.SectionProcess(section, sectionTypes, checkRoles);
			return db.UpdateAsync(section);
		}

		public Task DeleteSectionAsync(int sectionId)
		{
			return db.Sections.Where(x => x.Id == sectionId).DeleteAsync();
		}
	}
}