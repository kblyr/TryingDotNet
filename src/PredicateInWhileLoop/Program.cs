Console.WriteLine("Hello World");
var x = 0;

new Looper().While(MyPredicate);


bool MyPredicate()
{
    return x++ < 10;
}

class Looper
{
    public void While(Func<bool> predicate)
    {
        while(predicate())
        {
            Console.WriteLine("");
        }
    }
}