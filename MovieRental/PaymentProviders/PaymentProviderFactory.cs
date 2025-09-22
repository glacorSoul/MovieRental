namespace MovieRental.PaymentProviders
{
    public class PaymentProviderFactory
    {
        public IPaymentProvider? Create(string paymentProviderName)
        {
            paymentProviderName = paymentProviderName.ToLower();
            Type? type = typeof(PaymentProviderFactory).Assembly.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IPaymentProvider)))
                .FirstOrDefault(t => t.FullName!.ToLower().Contains(paymentProviderName));
            if (type is null)
            {
                return null;
            }
            IPaymentProvider? paymentProvider = Activator.CreateInstance(type) as IPaymentProvider;
            return paymentProvider;
        }
    }
}