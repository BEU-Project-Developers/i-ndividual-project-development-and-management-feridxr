using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using GalaSoft.MvvmLight.CommandWpf;

using Scheduler.Exceptions;
using Scheduler.Model.DBEntities;

namespace Scheduler.ViewModel
{
    public class CustomerViewModel : ViewModelBase
    {
        private List<Customer> _allcustomersloaded;
        private ObservableCollection<Address> _allAddresses;
        private ObservableCollection<City> _allCities;
        private ObservableCollection<Country> _allCountries;

        private Customer _selectedcustomer;
        private Address _selectedaddress;
        private City _selectedcity;
        private Country _selectedcountry;
        private Country _selectedCountryItem;
        private City _selectedCityItem;
        private Address _selectedAddressItem;
        private City _selectedAddressCityItem;
        private Country _selectedAddressCountryItem;

        private bool _addMode = false;
        private bool _editMode = false;
        private bool _viewMode = true;
        private Visibility _isAddressEditMode = Visibility.Collapsed;
        private object _tabControlSelectedItem;
        private bool _customerTabSelected;
        private bool _addressTabSelected;
        private bool _cityTabSelected;
        private bool _countryTabSelected;

        enum Mode
        {
            Add,
            Edit,
            View
        }

        private void SetMode(Mode mode)
        {
            if (mode == Mode.Add)
            {
                AddMode = true;
                EditMode = true;
                ViewMode = false;

                if (SelectedCustomer == null)
                {
                    SelectedCustomer = AllCustomers.FirstOrDefault();
                }
                IsAddressEditMode = Visibility.Visible;
            }
            if (mode == Mode.Edit)
            {
                EditMode = true;
                ViewMode = false;
                IsAddressEditMode = Visibility.Visible;
            }
            if (mode == Mode.View)
            {
                AddMode = false;
                EditMode = false;
                ViewMode = true;
                IsAddressEditMode = Visibility.Collapsed;
            }
        }

        // Country
        private void OnAddCustomerButton(Customer customer)
        {
            SetMode(Mode.Add);
        }

        private void OnEditCustomerButton(Customer customer)
        {
            SetMode(Mode.Edit);
        }

        private void OnDeleteCustomerButton(Customer customer)
        {
            if (MessageBox.Show("Are you sure you want to delete this customer?" +
                    "\r\n Id:" + customer.CustomerId +
                    "\r\n Name:" + customer.CustomerName,
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    CheckIfCustomerHasAppointments(customer);
                }
                catch (CustomerAttachedAppointmentException e)
                {
                    MessageBox.Show(e.Message);
                    return;
                }

                var context = new DBContext();
                context.Remove(customer);
                context.SaveChanges();
                SetMode(Mode.View);
                LoadCustomers();
            }
        }

        private void OnSaveCustomerButton(Customer customer)
        {
            var context = new DBContext();
            if (AddMode)
            {
                int NextId = AllCustomers.
                    OrderByDescending(a => a.CustomerId).FirstOrDefault().CustomerId + 1;

                Customer NewCustomer = new Customer
                {
                    //CustomerId = NextId,
                    CustomerName = customer.CustomerName,
                    AddressId = SelectedAddress.AddressId,
                    Active = customer.Active,
                    //Address = customer.Address,
                    CreateDate = DateTime.Now.ToUniversalTime(),
                    CreatedBy = customer.CreatedBy,
                    LastUpdate = DateTime.Now.ToUniversalTime(),
                    LastUpdateBy = customer.LastUpdateBy
                }; 
                context.Add(NewCustomer);
            }
            else
            {
                //customer.LastUpdate = DateTime.Now.ToUniversalTime();

                context.Update(customer);
                //context.Update(SelectedAddress);
                //context.Update(SelectedCity);
                //context.Update(SelectedCountry);
            }

            context.SaveChanges();
            LoadCustomers();
            SetMode(Mode.View);
        }

        private void OnCancelCustomerButton(Customer customer)
        {
            LoadCustomers();
            SetMode(Mode.View);
        }

        // Address
        private void OnAddAddressButton(Address address)
        {
            SetMode(Mode.Add);
        }

        private void OnEditAddressButton(Address address)
        {
            SetMode(Mode.Edit);
        }

        private void OnDeleteAddressButton(Address address)
        {
            if (MessageBox.Show("Are you sure you want to delete this address?" +
                    "\r\n Id: " + address.AddressId +
                    "\r\n Street: " + address.Address1 +
                    "\r\n Apt: " + address.Address2 + 
                    "\r\n City: " + address.City.City1 +
                    "\r\n Country: " + address.City.Country.Country1 +
                    "\r\n Zip: " + address.PostalCode, 
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var context = new DBContext();
                context.Remove(address);
                context.SaveChanges();
                SetMode(Mode.View);
                LoadAddresses();
            }
        }

        private void OnSaveAddressButton(Address address)
        {
            var context = new DBContext();
            if (AddMode)
            {
                int NextId = AllAddresses.
                    OrderByDescending(a => a.AddressId).FirstOrDefault().AddressId + 1;

                
                
                
                Address NewAddress = new Address
                {
                    //AddressId = NextId,
                    Address1 = address.Address1,
                    Address2 = address.Address2,
                    CityId = address.CityId,
                    Phone = address.Phone,
                    PostalCode = address.PostalCode,
                    CreateDate = DateTime.Now.ToUniversalTime(),
                    CreatedBy = address.CreatedBy,
                    LastUpdate = DateTime.Now.ToUniversalTime(),
                    LastUpdateBy = address.LastUpdateBy
                };
                context.Add(NewAddress);
            }
            else
            {
                //customer.LastUpdate = DateTime.Now.ToUniversalTime();

                context.Update(address);
                //context.Update(SelectedAddress);
                //context.Update(SelectedCity);
                //context.Update(SelectedCountry);
            }

            context.SaveChanges();
            LoadAddresses();
            SetMode(Mode.View);
            IsAddressEditMode = Visibility.Visible;
        }

        private void OnCancelAddressButton(Address address)
        {
            LoadAddresses();
            SetMode(Mode.View);
        }

        // City
        private void OnAddCityButton(City city)
        {
            SetMode(Mode.Add);
        }

        private void OnEditCityButton(City city)
        {
            SetMode(Mode.Edit);
        }

        private void OnDeleteCityButton(City city)
        {
            if (MessageBox.Show("Are you sure you want to delete this city?" +
                    "\r\n Id:" + city.CityId +
                    "\r\n Name:" + city.City1,
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var context = new DBContext();
                context.Remove(city);
                context.SaveChanges();
                SetMode(Mode.View);
                LoadCities();
            }
        }

        private void OnSaveCityButton(City city)
        {
            var context = new DBContext();
            if (AddMode)
            {
                int NextId = AllCities.
                    OrderByDescending(a => a.CityId).FirstOrDefault().CityId + 1;

                City NewCity = new City
                {
                    //CityId = NextId,
                    City1 = city.City1,
                    CountryId = city.CountryId,
                    CreateDate = DateTime.Now.ToUniversalTime(),
                    CreatedBy = city.CreatedBy,
                    LastUpdate = DateTime.Now.ToUniversalTime(),
                    LastUpdateBy = city.LastUpdateBy
                };
                context.Add(NewCity);
            }
            else
            {
                //customer.LastUpdate = DateTime.Now.ToUniversalTime();

                context.Update(city);
                //context.Update(SelectedAddress);
                //context.Update(SelectedCity);
                //context.Update(SelectedCountry);
            }

            context.SaveChanges();
            LoadCities();
            SetMode(Mode.View);
        }

        private void OnCancelCityButton(City city)
        {
            LoadCities();
            SetMode(Mode.View);
        }

        // Country
        private void OnAddCountryButton(Country country)
        {
            SetMode(Mode.Add);
        }

        private void OnEditCountryButton(Country country)
        {
            SetMode(Mode.Edit);
        }

        private void OnDeleteCountryButton(Country country)
        {
            if (MessageBox.Show("Are you sure you want to delete this country?" +
                    "\r\n Id:" + country.CountryId +
                    "\r\n Name:" + country.Country1,
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var context = new DBContext();
                context.Remove(country);
                context.SaveChanges();
                SetMode(Mode.View);
                LoadCountries();
            }
        }

        private void OnSaveCountryButton(Country country)
        {
            var context = new DBContext();
            if (AddMode)
            {
                int NextId = AllCountries.
                    OrderByDescending(a => a.CountryId).FirstOrDefault().CountryId + 1;

                Country NewCountry = new Country
                {
                    //CountryId = NextId,
                    Country1 = country.Country1,
                    CreateDate = DateTime.Now.ToUniversalTime(),
                    CreatedBy = country.CreatedBy,
                    LastUpdate = DateTime.Now.ToUniversalTime(),
                    LastUpdateBy = country.LastUpdateBy
                };
                context.Add(NewCountry);
            }
            else
            {
                //customer.LastUpdate = DateTime.Now.ToUniversalTime();
                //AllCities.Where(a => a.CountryId == country.CountryId).
                context.Update(country);
                //context.Update(SelectedAddress);
                //context.Update(SelectedCity);
                //context.Update(SelectedCountry);
            }
            context.SaveChanges();
            LoadCountries();
            SetMode(Mode.View);
        }

        private void OnCancelCountryButton(Country country)
        {
            LoadCountries();
            SetMode(Mode.View);
        }

        public CustomerViewModel()
        {
            AddCustomerCommand = new RelayCommand<Customer>(OnAddCustomerButton);
            EditCustomerCommand = new RelayCommand<Customer>(OnEditCustomerButton);
            DeleteCustomerCommand = new RelayCommand<Customer>(OnDeleteCustomerButton);
            SaveCustomerCommand = new RelayCommand<Customer>(OnSaveCustomerButton);
            CancelCustomerCommand = new RelayCommand<Customer>(OnCancelCustomerButton);

            AddAddressCommand = new RelayCommand<Address>(OnAddAddressButton);
            EditAddressCommand = new RelayCommand<Address>(OnEditAddressButton);
            DeleteAddressCommand = new RelayCommand<Address>(OnDeleteAddressButton);
            SaveAddressCommand = new RelayCommand<Address>(OnSaveAddressButton);
            CancelAddressCommand = new RelayCommand<Address>(OnCancelAddressButton);

            AddCityCommand = new RelayCommand<City>(OnAddCityButton);
            EditCityCommand = new RelayCommand<City>(OnEditCityButton);
            DeleteCityCommand = new RelayCommand<City>(OnDeleteCityButton);
            SaveCityCommand = new RelayCommand<City>(OnSaveCityButton);
            CancelCityCommand = new RelayCommand<City>(OnCancelCityButton);

            AddCountryCommand = new RelayCommand<Country>(OnAddCountryButton);
            EditCountryCommand = new RelayCommand<Country>(OnEditCountryButton);
            DeleteCountryCommand = new RelayCommand<Country>(OnDeleteCountryButton);
            SaveCountryCommand = new RelayCommand<Country>(OnSaveCountryButton);
            CancelCountryCommand = new RelayCommand<Country>(OnCancelCountryButton);

            LoadCountries();
            LoadCities();
            LoadAddresses();
            LoadCustomers();

        }
        // Customer
        public RelayCommand<string> GetCustomersCommand { get; private set; }
        public RelayCommand<Customer> AddCustomerCommand { get; private set; }
        public RelayCommand<Customer> EditCustomerCommand { get; private set; }
        public RelayCommand<Customer> DeleteCustomerCommand { get; private set; }
        public RelayCommand<Customer> SaveCustomerCommand { get; private set; }
        public RelayCommand<Customer> CancelCustomerCommand { get; private set; }

        // Address
        public RelayCommand<Address> AddAddressCommand { get; private set; }
        public RelayCommand<Address> EditAddressCommand { get; private set; }
        public RelayCommand<Address> DeleteAddressCommand { get; private set; }
        public RelayCommand<Address> SaveAddressCommand { get; private set; }
        public RelayCommand<Address> CancelAddressCommand { get; private set; }

        //City
        public RelayCommand<City> AddCityCommand { get; private set; }
        public RelayCommand<City> EditCityCommand { get; private set; }
        public RelayCommand<City> DeleteCityCommand { get; private set; }
        public RelayCommand<City> SaveCityCommand { get; private set; }
        public RelayCommand<City> CancelCityCommand { get; private set; }

        // Country
        public RelayCommand<Country> AddCountryCommand { get; private set; }
        public RelayCommand<Country> EditCountryCommand { get; private set; }
        public RelayCommand<Country> DeleteCountryCommand { get; private set; }
        public RelayCommand<Country> SaveCountryCommand { get; private set; }
        public RelayCommand<Country> CancelCountryCommand { get; private set; }


        public bool AddMode
        {
            get => _addMode;
            set
            {
                if (value != _addMode)
                {
                    SetProperty(ref _addMode, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool ViewMode
        {
            get => _viewMode;
            set
            {
                if (value != _viewMode)
                {
                    SetProperty(ref _viewMode, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool EditMode
        {
            get => _editMode;
            set
            {
                if (value != _editMode)
                {
                    SetProperty(ref _editMode, value);
                    OnPropertyChanged();
                }
            }
        }

        public Visibility IsAddressEditMode
        {
            get => _isAddressEditMode;
            set
            {
                if (value != _isAddressEditMode)
                {
                    SetProperty(ref _isAddressEditMode, value);
                    OnPropertyChanged();
                }
            }
        }

        public List<Customer> AllCustomers
        {
            get
            {
                var context = new DBContext();
                return context.Customer.ToList();
            }
            set
            {
                var context = new DBContext();
                context.Customer.UpdateRange(value.ToList());
                context.SaveChanges();
            }
        }

        public List<Customer> AllCustomersLoaded
        {
            get => _allcustomersloaded;
            set
            {
                if (value != _allcustomersloaded)
                {
                    SetProperty(ref _allcustomersloaded, value);
                    OnPropertyChanged();
                }
            }
        }

        public void LoadCustomers()
        {
            var context = new DBContext();
            AllCustomersLoaded = context.Customer.ToList();
        }

        public void LoadAddresses()
        {
            var context = new DBContext();
            //var allAddresses = context.Address.ToList();
            AllAddresses = new ObservableCollection<Address>(context.Address.ToList());
        }

        public void LoadCities()
        {
            var context = new DBContext();
            //var allCities = context.City.ToList();
            AllCities = new ObservableCollection<City>(context.City.ToList());
        }

        public void LoadCountries()
        {
            var context = new DBContext();
            //var allCountries = await context.Country.ToListAsync();
            AllCountries = new ObservableCollection<Country>(context.Country.ToList());
        }

        public Customer SelectedCustomer
        {
            get => _selectedcustomer;
            set
            {
                if (value != null && value != _selectedcustomer)
                {
                    var context = new DBContext();
                    value.Address = context.Address.Find(value.AddressId);
                    value.Address.City = context.City.Find(value.Address.CityId);
                    value.Address.City.Country = context.Country.Find(value.Address.City.CountryId);

                    SetProperty(ref _selectedcustomer, value);

                    //SelectedAddress = context.Address.Find(value.AddressId);

                    OnPropertyChanged();

                    SelectedAddress = value.Address;
                }
            }
        }

        public Address SelectedAddress
        {
            get => _selectedaddress;
            set
            {
                if (value != null && value != _selectedaddress)
                {
                    var context = new DBContext();
                    value.City = context.City.Find(value.CityId);
                    value.City.Country = context.Country.Find(value.City.CountryId);

                    SetProperty(ref _selectedaddress, value);

                    SelectedCity = context.City.Find(value.CityId);
                    SelectedCustomer.Address = value;

                    OnPropertyChanged();
                }
            }
        }

        public City SelectedCity
        {
            get => _selectedcity;
            set
            {
                if (value != null && value != _selectedcity)
                {
                    SetProperty(ref _selectedcity, value);

                    var context = new DBContext();
                    SelectedCountry = context.Country.Find(value.CountryId);

                    OnPropertyChanged();
                }
            }
        }

        public Country SelectedCountry
        {
            get => _selectedcountry;
            set
            {
                if (value != null && value != _selectedcountry)
                {
                    SetProperty(ref _selectedcountry, value);
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Address> AllAddresses
        {
            get => _allAddresses;
            set
            {
                if (value != _allAddresses)
                {
                    SetProperty(ref _allAddresses, value);
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<City> AllCities
        {
            get => _allCities;
            set
            {
                if (value != _allCities)
                {
                    SetProperty(ref _allCities, value);
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Country> AllCountries

        {
            get => _allCountries;
            set
            {
                if (value != _allCountries)
                {
                    SetProperty(ref _allCountries, value);
                    OnPropertyChanged();
                }
            }
        }

        public Country SelectedCountryItem
        {
            get => _selectedCountryItem;
            set
            {
                if (value != null && value != _selectedCountryItem)
                {
                    SetProperty(ref _selectedCountryItem, value);
                    OnPropertyChanged();
                }
            }
        }

        public City SelectedCityItem
        {
            get => _selectedCityItem;
            set
            {
                if (value != null && value != _selectedCityItem)
                {
                    SetProperty(ref _selectedCityItem, value);
                    OnPropertyChanged();
                }
            }
        }

        public Address SelectedAddressItem
        {
            get => _selectedAddressItem;
            set
            {
                if (value != null && value != _selectedAddressItem)
                {
                    var context = new DBContext();
                    value.City = context.City.Find(value.CityId);
                    value.City.Country = context.Country.Find(value.City.CountryId);

                    SetProperty(ref _selectedAddressItem, value);
                    OnPropertyChanged();

                    //SelectedAddressCityItem = null;
                    //SelectedAddressCountryItem = null;
                }
            }
        }

        public City SelectedAddressCityItem
        {
            get => _selectedAddressCityItem;
            set
            {
                if (value != null && value != _selectedAddressCityItem)
                {
                    //SelectedAddressItem.CityId = value.CityId;
                    SetProperty(ref _selectedAddressCityItem, value);
                    OnPropertyChanged();

                    //SelectedAddressItem = null;
                    //SelectedAddressCountryItem = null;
                    //if (SelectedAddressItem != null && SelectedAddressItem.CityId != value.CityId)
                    //{
                    //SelectedAddressItem.CityId = value.CityId;
                    //SelectedAddressItem.City = AllCities[value.CityId];
                    //}
                }
            }
        }

        public Country SelectedAddressCountryItem
        {
            get => _selectedAddressCountryItem;
            set
            {
                if (value != null && value != _selectedAddressCountryItem)
                {
                    //SelectedAddressItem.City.CountryId = value.CountryId;
                    SetProperty(ref _selectedAddressCountryItem, value);
                    OnPropertyChanged();

                    //SelectedAddressItem = null;
                    //SelectedAddressCityItem = null;
                    //if (SelectedAddressItem != null && SelectedAddressItem.City.CountryId != value.CountryId)
                    //{
                    //SelectedAddressItem.City.CountryId = value.CountryId;
                    //SelectedAddressItem.City.Country = AllCountries[value.CountryId];
                    //}
                }
            }
        }

        public object TabControlSelectedItem
        {
            get => _tabControlSelectedItem;
            set
            {
                if (value != _tabControlSelectedItem)
                {
                    SetProperty(ref _tabControlSelectedItem, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool CustomerTabSelected
        {
            get => _customerTabSelected;
            set
            {
                if (value != _customerTabSelected)
                {
                    SetProperty(ref _customerTabSelected, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool AddressTabSelected
        {
            get => _addressTabSelected;
            set
            {
                if (value != _addressTabSelected)
                {
                    SetProperty(ref _addressTabSelected, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool CityTabSelected
        {
            get => _cityTabSelected;
            set
            {
                if (value != _cityTabSelected)
                {
                    SetProperty(ref _cityTabSelected, value);
                    OnPropertyChanged();
                }
            }
        }

        public bool CountryTabSelected
        {
            get => _countryTabSelected;
            set
            {
                if (value != _countryTabSelected)
                {
                    SetProperty(ref _countryTabSelected, value);
                    OnPropertyChanged();
                }
            }
        }

        public List<Appointment> AllAppointments
        {
            get
            {
                var context = new DBContext();
                List<Appointment> appointments = context.Appointment.ToList();
                foreach (Appointment appointment in appointments)
                {
                    appointment.Start = appointment.Start.ToLocalTime();
                    appointment.End = appointment.End.ToLocalTime();
                }

                return appointments;
            }
            set
            {
                var context = new DBContext();
                context.Appointment.UpdateRange(value.ToList());
                context.SaveChanges();
            }
        }

        public void CheckIfCustomerHasAppointments(Customer customer)
        {
            var attachedAppointment = AllAppointments.Where(a => a.CustomerId == customer.CustomerId);
            if (attachedAppointment.Any())
            {
                Appointment appointment = attachedAppointment.First();
                throw (new CustomerAttachedAppointmentException(
                    $"Unable to delete '{customer.CustomerName}', as\r\n" +
                    $"customer is attached to an existing appointment.\r\n" +
                    $"Conflicting Appointment:\r\n" +
                    $"ID: {appointment.AppointmentId}\r\n" +
                    $"Start: {appointment.Start}\r\n" +
                    $"End: {appointment.End}"
                ));
            }
        }

    }
}
