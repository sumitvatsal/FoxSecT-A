using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FoxSec.Common.Extensions;

namespace FoxSec.DomainModel.DomainObjects
{
    public enum Menu
    {
        [Display(Name = "Administration menu")]
        ViewAdministrationMenu = 1,
        [Display(Name = "Time Zones menu")]
        ViewTimeZoneMenu = 2,
        [Display(Name = "Permissions menu")]
        ViewPermissionMenu = 3,
        [Display(Name = "Users menu")]
        ViewPeopleMenu = 4,
        [Display(Name = "Cards menu")]
        ViewCardsMenu = 5,
        [Display(Name = "Log menu")]
        ViewLogMenu = 6,
        [Display(Name = "My Account menu")]
        ViewMyAccountMenu = 7,
        [Display(Name = "Administration -> Role Management menu")]
        ViewRoleManadgmentMenu = 8,
        [Display(Name = "Administration -> Titles menu")]
        ViewTitleMenu = 9,
        [Display(Name = "Administration -> Companies menu")]
        ViewCompanyMenu = 10,
        [Display(Name = "Administration -> Departments menu")]
        ViewDepartmentMenu = 11,
        [Display(Name = "Administration -> Holidays menu")]
        ViewHolidayMenu = 12,
        [Display(Name = "Administration -> Settings menu")]
        ViewClassifierMenu = 13,
        [Display(Name = "Administration -> Card Types menu")]
        ViewCardTypeMenu = 14,
        [Display(Name = "Administration -> Buildings menu")]
        ViewBuildingsMenu = 15,
        [Display(Name = "Administration -> My Company menu")]
        ViewMyCompanyMenu = 16,
        [Display(Name = "Location menu")]
        ViewMyLocationMenu = 17,
        [Display(Name = "T&A")]
        ViewTAMenu = 18,
        [Display(Name = "T&A -> Configuration")]
        ViewTAConfMenu = 19,
        [Display(Name = "T&A -> Report")]
        ViewTAReportMenu = 20,
        [Display(Name = "Visitors")]
        ViewVisitorsMenu = 21,
        [Display(Name = "Reports")]
        ViewMyReportsMenu = 22,
        [Display(Name = "Live Video")]
        LiveVideo = 23,
        [Display(Name = "Bo Info")]
        ViewMyBOInfoMenu = 24,
        [Display(Name = "Delete Permission Group From Users")]
        ViewDeletePermGroupMenu = 25,
        [Display(Name = "Terminals")]
        ViewMyTerminalMenu = 26,
        [Display(Name = "Bo Scedule")]
        ViewMyBOSceduleMenu = 27,
        [Display(Name = "View Visiotrs")]
        ViewVisitorsReadOnly = 28,
        [Display(Name = "System configuration")]
        SystemConfiguration = 29,
       [Display(Name = "T&A Register")]
        TARegister = 30,
    }

    public interface IMenuSet : IEnumerable<KeyValuePair<Menu, bool>>
    {
        bool this[Menu m] { get; set; }
        bool IsAvailabe(int index);
        byte[] GetByteArray();
        IMenuSet AsReadOnly();
    }

    public class MenuSet : IMenuSet
    {
        private readonly IDictionary<Menu, bool> _MenuMap;
        private readonly byte[] _Menus;

        public MenuSet(byte[] Menus)
        {
            _MenuMap = new Dictionary<Menu, bool>(Menus.Length - 1);
            _Menus = Menus;
            InitializeMenuMap(Menus, _MenuMap);
        }

        private static void InitializeMenuMap(byte[] Menus, IDictionary<Menu, bool> MenuMap)
        {
            for (int i = 0; i < Menus.Length - 1; i++)
            {
                var m = i.AsEnum<Menu>();

                if (Enum.IsDefined(typeof(Menu), m))
                {
                    MenuMap.Add(m, Menus[i] != 0);
                }
            }
        }

        public bool this[Menu m]
        {
            get
            {
                bool granted;
                _MenuMap.TryGetValue(m, out granted);
                return granted;
            }

            set
            {
                _MenuMap[m] = value;
                _Menus[(int)m] = (byte)(value ? 1 : 0);
            }
        }

        public bool IsAvailabe(int index)
        {
            return _Menus[index] == 1;
        }

        public static IMenuSet Merge(IMenuSet left, IMenuSet right)
        {
            var result = new MenuSet(left.GetByteArray());

            foreach (var Menu in left)
            {
                result[Menu.Key] = result[Menu.Key] || right[Menu.Key];
            }

            return result;
        }

        public byte[] GetByteArray()
        {
            return _Menus;
        }

        public IMenuSet AsReadOnly()
        {
            return new MenuSetReadOnly(this);
        }

        public IEnumerator<KeyValuePair<Menu, bool>> GetEnumerator()
        {
            return _MenuMap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    internal class MenuSetReadOnly : IMenuSet
    {
        private readonly MenuSet _MenuSet;

        public MenuSetReadOnly(MenuSet MenuSet)
        {
            _MenuSet = MenuSet;
        }

        public bool this[Menu m]
        {
            get { return _MenuSet[m]; }
            set { throw ThrowOnMutation(); }
        }

        public bool IsAvailabe(int index)
        {
            return _MenuSet[index.AsEnum<Menu>()];
        }

        public byte[] GetByteArray()
        {
            throw ThrowOnMutation();
        }

        public IMenuSet AsReadOnly()
        {
            return this;
        }

        public IEnumerator<KeyValuePair<Menu, bool>> GetEnumerator()
        {
            return _MenuSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static Exception ThrowOnMutation()
        {
            return new InvalidOperationException("Operation is not allowed on read-only Menu set.");
        }
    }
}