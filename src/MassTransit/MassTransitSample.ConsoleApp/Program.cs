using MassTransit;
using MassTransitSample.ConsoleApp;

var busControl = Bus.Factory.CreateUsingInMemory(config => {
    config.ReceiveEndpoint(endpoint => {
        endpoint.Consumer<ValueEnteredConsumer>();
    });
});

await busControl.StartAsync();

try
{
    Console.WriteLine(typeof(ValueEntered).FullName);

    while (true) {
        var value = await Task.Run(() => {
            Console.WriteLine("Enter message (or quit to exit)");
            Console.Write("> ");
            return Console.ReadLine();
        });

        if (value is null)
            continue;

        if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
            break;

        await busControl.Publish<ValueEntered>(new {
            Value = value ?? ""
        });
    }
}
finally
{
    await busControl.StopAsync();
}

namespace MassTransitSample.ConsoleApp
{
    public interface ValueEntered
    {
        string Value { get; }
    }

    class ValueEnteredConsumer : IConsumer<ValueEntered>
    {
        public async Task Consume(ConsumeContext<ValueEntered> context)
        {
            Console.WriteLine("Value: {0}", context.Message.Value);
        }
    }
}