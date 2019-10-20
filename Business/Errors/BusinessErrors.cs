using Domain.Contracts.Errors;
using Domain.Errors;

namespace Business.Errors
{
    public static class BusinessErrors
    {
        public static ErrorMessage InvalidInput
        {
            get
            {
                return new ErrorMessage("Service01", "Invalid Input.", ErrorTypes.Validation);
            }
        }

        public static ErrorMessage ProductPriceRequiresConfirmation
        {
            get
            {
                return new ErrorMessage("Service02", "Price exceeds the allowed value. Please, confirm.", ErrorTypes.Warning);
            }
        }

        public static ErrorMessage ProductCodeAlreadyExists
        {
            get
            {
                return new ErrorMessage("Service03", "The entered product code is already in use.", ErrorTypes.Validation);
            }
        }

        public static ErrorMessage ProductNotFound
        {
            get
            {
                return new ErrorMessage("Service04", "Could not found the selected product.", ErrorTypes.Validation);
            }
        }
    }
}
