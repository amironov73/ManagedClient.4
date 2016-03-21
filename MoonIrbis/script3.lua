local f = assert(io.open ("hello.txt", "r"))
local t = f:read("*a")
print (t)
f:close()
