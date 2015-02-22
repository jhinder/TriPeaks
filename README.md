# TriPeaks
TriPeaks is a modern clone of the TriPeaks game from the Microsoft Entertainment pack for Windows 3.1, written in C# .NET and WPF.  
The original programs are 16 bit programs and don't run under newer Windows versions or on any 64 bit Windows versions.

Originally the game was supposed to be as close to the orginal program as possible, but now the goal is to modernise the game while keeping the rules of the original.

## Requirements
To run and compile the program, .NET 4.5 is required.  
The program was written with Visual Studio 2013, although 2012 should also work.

It could theoretically run under earlier versions of the .NET Framework, but then you'd lose all asynchronicity, and the code makes use of TPL & async quite a bit.

## Licence
The program is licenced under the MIT licence. See the license file for details.
