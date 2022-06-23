using PIS.Profiler.Repository;

Console.WriteLine("StartCall");
dynamic entity = new Person("Jon","Depp");
entity.Save(entity);
Console.WriteLine("EndCall");
