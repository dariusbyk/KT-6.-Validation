using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace ValidationDemo
{
    public class User : INotifyDataErrorInfo
    {
        private string email = "";
        private string password = "";
        private string age = "";

        private Dictionary<string, List<string>> errors =
            new Dictionary<string, List<string>>();

        public User()
        {
            ValidateAll();
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                ValidateEmail();
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                ValidatePassword();
            }
        }

        // Храним строку, чтобы самостоятельно проверить
        // и пустое поле, и ввод букв вместо числа.
        public string Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
                ValidateAge();
            }
        }

        public bool HasErrors
        {
            get
            {
                return errors.Count > 0;
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private void ValidateEmail()
        {
            errors.Remove(nameof(Email));

            if (string.IsNullOrWhiteSpace(email))
            {
                AddError(nameof(Email), "Введите email.");
            }
            else if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                AddError(
                    nameof(Email),
                    "Пример правильного email: user@mail.ru."
                );
            }

            OnErrorsChanged(nameof(Email));
        }

        private void ValidatePassword()
        {
            errors.Remove(nameof(Password));

            if (string.IsNullOrWhiteSpace(password))
            {
                AddError(nameof(Password), "Введите пароль.");
            }
            else if (password.Length < 6)
            {
                AddError(
                    nameof(Password),
                    "Пароль должен содержать минимум 6 символов."
                );
            }

            OnErrorsChanged(nameof(Password));
        }

        private void ValidateAge()
        {
            errors.Remove(nameof(Age));

            int number;

            if (string.IsNullOrWhiteSpace(age))
            {
                AddError(nameof(Age), "Введите возраст.");
            }
            else if (int.TryParse(age, out number) == false)
            {
                AddError(nameof(Age), "Возраст должен быть целым числом.");
            }
            else if (number < 18 || number > 100)
            {
                AddError(nameof(Age), "Возраст должен быть от 18 до 100.");
            }

            OnErrorsChanged(nameof(Age));
        }

        private void AddError(string propertyName, string message)
        {
            if (errors.ContainsKey(propertyName) == false)
            {
                errors[propertyName] = new List<string>();
            }

            errors[propertyName].Add(message);
        }

        private void OnErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
            {
                ErrorsChanged(
                    this,
                    new DataErrorsChangedEventArgs(propertyName)
                );
            }
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            // Если конкретное свойство не указано,
            // возвращаем все ошибки.
            if (string.IsNullOrEmpty(propertyName))
            {
                List<string> allErrors = new List<string>();

                foreach (List<string> propertyErrors in errors.Values)
                {
                    allErrors.AddRange(propertyErrors);
                }

                return allErrors;
            }

            if (errors.ContainsKey(propertyName))
            {
                return errors[propertyName];
            }

            return new List<string>();
        }

        public void ValidateAll()
        {
            ValidateEmail();
            ValidatePassword();
            ValidateAge();
        }
    }
}