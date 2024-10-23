local config =  {
    config = {
        ["1"] = {name = "底红", color = "#e67373", group = 1, red = 2, yellow = 0, blue = 0},
        ["2"] = {name = "中红", color = "#e61717", group = 1, red = 3, yellow = 0, blue = 0},
        ["3"] = {name = "高红", color = "#b31212", group = 1, red = 4, yellow = 0, blue = 0},

        ["4"] = {name = "低黄", color = "#fff7b2", group = 2, red = 0, yellow = 2, blue = 0},
        ["5"] = {name = "中黄", color = "#ffeb33", group = 2, red = 0, yellow = 3, blue = 0},
        ["6"] = {name = "高黄", color = "#e6d435", group = 2, red = 0, yellow = 4, blue = 0},

        ["7"] = {name = "低蓝", color = "#aaddf4", group = 3, red = 0, yellow = 0, blue = 2},
        ["8"] = {name = "中蓝", color = "#18aef4", group = 3, red = 0, yellow = 0, blue = 3},
        ["9"] = {name = "高蓝", color = "#039ee6", group = 3, red = 0, yellow = 0, blue = 4},

        ["10"] = {name = "低橙", color = "#ffc266", group = 4, red = 1, yellow = 1, blue = 0},
        ["11"] = {name = "中橙", color = "#18aef4", group = 4, red = 2, yellow = 2, blue = 0},
        ["12"] = {name = "橙黄", color = "#ff9900", group = 4, red = 1, yellow = 2, blue = 0},
        ["13"] = {name = "橙红", color = "#ff7300", group = 4, red = 2, yellow = 1, blue = 0},

        ["14"] = {name = "低绿", color = "#aee681", group = 5, red = 0, yellow = 1, blue = 1},
        ["15"] = {name = "中绿", color = "#74e617", group = 5, red = 0, yellow = 2, blue = 2},
        ["16"] = {name = "黄绿", color = "#b8ed17", group = 5, red = 0, yellow = 2, blue = 1},
        ["17"] = {name = "蓝绿", color = "#00ab87", group = 5, red = 0, yellow = 1, blue = 2},

        ["18"] = {name = "低紫", color = "#a56ab0", group = 6, red = 1, yellow = 0, blue = 1},
        ["19"] = {name = "中紫", color = "#9c27b0", group = 6, red = 2, yellow = 0, blue = 2},
        ["20"] = {name = "紫红", color = "#cb29b0", group = 6, red = 2, yellow = 0, blue = 1},
        ["21"] = {name = "紫蓝", color = "#8e4cff", group = 6, red = 1, yellow = 0, blue = 2},

        ["22"] = {name = "纯黑", color = "#000000", group = 7, red = 1, yellow = 1, blue = 1},
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