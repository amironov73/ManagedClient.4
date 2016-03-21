local record = CreateRecord ()
local field = CreateField ("200")
field.AddSubField ('a', "Это тест")
record.Fields.Add (field)

print (record.ToPlainText())