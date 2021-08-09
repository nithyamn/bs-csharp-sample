python -m SimpleHTTPServer 8888 &
pid=$!
dotnet build
dotnet test
kill $pid
