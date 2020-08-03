-- constants / var that will change but it's mostly config stuff
if not isfile("nearby_notif.cfg") then
    writefile("nearby_notif.cfg","3")
end
NEAR_DISTANCE = tonumber(readfile("nearby_notif.cfg")) -- Distance that is considered to be near a player
-- constants vars
local LocalPlayer = game.Players.LocalPlayer
local OnNewMessage = game.ReplicatedStorage.DefaultChatSystemChatEvents.OnMessageDoneFiltering
local Remotes = game.ReplicatedStorage.Remotes
local HttpService  = game:GetService("HttpService")


-- functions
function get_distance_within_localplayer( player )
    return LocalPlayer.Character.HumanoidRootPart.Position.Magnitude - game.Players[player].Character.HumanoidRootPart.Position.Magnitude
end

function is_within_distance( player )
    if NEAR_DISTANCE >= get_distance_within_localplayer(player) then
        
        return true
    else 
        return true;
    end
end

function get_player_rap(user_id)
    return Remotes.UserLimiteds.GetUserRAP:InvokeServer(user_id).rap
end

function get_user_value( user_id )
    return Remotes.UserLimiteds.GetUserRAP:InvokeServer(user_id).value
end


--// websocket connections
local onMessageSocket = syn.websocket.connect("ws://localhost:24892/custom?channel=ChatSocket")
local saveSettingSocket = syn.websocket.connect("ws://localhost:24892/custom?channel=settingSaveRequest")
local PlayerDataSocket = syn.websocket.connect("ws://localhost:24892/custom?channel=PlayerDataSocket")

saveSettingSocket.OnMessage:connect(function(new_near_distance)
    if not tonumber(new_near_distance) then
        saveSettingSocket:Send("LETTER_ERROR")
    else
        NEAR_DISTANCE = tonumber(new_near_distance);
        writefile("nearby_notif.cfg", new_near_distance)
    end
end)

OnNewMessage.OnClientEvent:connect(function(MessageTable,MessageChannel)
    if MessageTable.FromSpeaker == LocalPlayer.Name then return end
    local SpeakerInstance = game.Players[MessageTable.FromSpeaker]
    local SpeakerUserId = MessageTable.SpeakerUserId;
    local SpeakerName = MessageTable.FromSpeaker
    local SpeakerRAP = get_player_rap(SpeakerUserId)
    local SpeakerValue = get_user_value(SpeakerUserId)
    if is_within_distance(SpeakerName) then
        local SpeakerData = HttpService:JSONEncode{
            Username = SpeakerName,
            UserId = SpeakerUserId,
            RecentAveragePrice = SpeakerRAP,
            Value = SpeakerValue;
            AvatarThumb = "https://www.roblox.com/Thumbs/Avatar.ashx?x=420&y=420&userid=" .. SpeakerUserId
        } 
        PlayerDataSocket:Send(SpeakerData)
    end
end)