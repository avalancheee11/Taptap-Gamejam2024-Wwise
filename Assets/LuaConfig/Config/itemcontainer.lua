local config =  {
    config = {
        ["1"] = {capacity = 1, url = "cardIcon3",interurl = "cardIcon3_inter"},
        ["2"] = {capacity = 2, url = "cardIcon3",interurl = "cardIcon3_inter"},
        ["3"] = {capacity = 3, url = "cardIcon3",interurl = "cardIcon3_inter"},
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