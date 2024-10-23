local config =  {
    config = {
        ["3"] = {name = "小红果", url = "cardIcon1", red = 1, yellow = 0, blue = 0, itemType = "Normal"},
        ["4"] = {name = "小黄果", url = "cardIcon1", red = 0, yellow = 1, blue = 0, itemType = "Normal"},
        ["5"] = {name = "小蓝果", url = "cardIcon1", red = 0, yellow = 0, blue = 1, itemType = "Normal"},
        ["6"] = {name = "橙色块", url = "cardIcon1", red = 1, yellow = 1, blue = 0, itemType = "Normal"},
        ["7"] = {name = "绿色块", url = "cardIcon1", red = 0, yellow = 1, blue = 1, itemType = "Normal"},
        ["8"] = {name = "紫色块", url = "cardIcon1", red = 1, yellow = 0, blue = 1, itemType = "Normal"},
        ["9"] = {name = "红晶石", url = "cardIcon1", red = 2, yellow = 0, blue = 0, itemType = "Normal"},
        ["10"] = {name = "黄晶石", url = "cardIcon1", red = 0, yellow = 2, blue = 0, itemType = "Normal"},
        ["11"] = {name = "蓝晶石", url = "cardIcon1", red = 0, yellow = 0, blue = 2, itemType = "Normal"},


        ["100"] = {name = "添加剂1【闪电】", url = "cardIcon1", itemType = "Special"},
        ["101"] = {name = "添加剂2【星星】", url = "cardIcon1", itemType = "Special"},
        ["102"] = {name = "添加剂3【蝴蝶】", url = "cardIcon1", itemType = "Special"},
        ["103"] = {name = "添加剂4【音符】", url = "cardIcon1", itemType = "Special"},
        ["104"] = {name = "添加剂5【雪花】", url = "cardIcon1", itemType = "Special"},
        ["105"] = {name = "添加剂6【翅膀】", url = "cardIcon1", itemType = "Special"},
    },
}
 
targetPath = arg[1];
fileName = arg[2];

--数据转json
function convert(val)
    fileDir = io.open(targetPath, "rb");
    if fileDir then 
        fileDir:close();
    else
        os.execute("mkdir -p ".. targetPath);
    end
    local cjson = require "cjson"
    local jsonData = cjson.encode(val)
    --保存
    local file = io.open(targetPath.."/"..fileName, "w");
    file:write(jsonData);
    file:close();

    return jsonData;
end

convert(config);