using System;
using System.Device.Gpio;
using System.Device.Gpio.Drivers;

namespace dotnet_led_button
{
    class Program
    {        
        private const int GPIOCHIP = 1;
        private const int LED_PIN = 36;
        private const int BUTTON_PIN = 38;       
        private static PinValue ledPinValue = PinValue.High;   
        //private static PinValue ledPinValue = PinValue.Low;     
        static void Main(string[] args)
        
        {                        
            GpioController controller;
            var drvGpio = new LibGpiodDriver(GPIOCHIP);
            controller = new GpioController(PinNumberingScheme.Logical, drvGpio);
            //set value
            if(!controller.IsPinOpen(LED_PIN)&&!controller.IsPinOpen(BUTTON_PIN))
            {
                controller.OpenPin(LED_PIN,PinMode.Output);
                controller.OpenPin(BUTTON_PIN,PinMode.Input);

            } else
            {
                Console.WriteLine("LED_PIN or BUTTON_PIN is busy");
                Environment.Exit(-1);
            }
            controller.Write(LED_PIN,ledPinValue); //LED OFF
            //
            Console.WriteLine("CTRL+C to interrupt the read operation:");
            controller.RegisterCallbackForPinValueChangedEvent(BUTTON_PIN,PinEventTypes.Falling,(o, e) =>
                {
                    ledPinValue=!ledPinValue;
                    controller.Write(LED_PIN,ledPinValue);
                    Console.WriteLine($"Press button, LED={ledPinValue}");        
                });
            //Console
            ConsoleKeyInfo cki;
            
            while (true)
            {
                Console.Write("Press any key, or 'X' to quit, or ");
                Console.WriteLine("CTRL+C to interrupt the read operation:");

                // Start a console read operation. Do not display the input.
                cki = Console.ReadKey(true);

                // Announce the name of the key that was pressed .
                Console.WriteLine($"  Key pressed: {cki.Key}\n");

                // Exit if the user pressed the 'X' key.
                if (cki.Key == ConsoleKey.X) break;
            }          
        }
    }
}