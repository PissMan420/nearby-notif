-- constants
NEAR_DISTANCE = 3 -- Distance that is considered to be near a player
-- imprt vars
local LocalPlayer = game.Players.LocalPlayer
local OnNewMessage = game.ReplicatedStorage.DefaultChatSystemChatEvents.OnMessageDoneFiltering
-- functions
function get_distance_within_localplayer( player )
    return LocalPlayer.Character.HumanoidRootPart.Position.Magnitude - game.Players[player].Character.HumanoidRootPart.Position.Magnitude
end

function is_within_distance( player )
    return get_distance_within_localplayer(player) < NEAR_DISTANCE
end

--// websocket connections
t = tick()
local onMessageSocket = syn.websocket.connect("ws://localhost:24892/custom?channel=ChatSocket")
local saveSettingSocket = syn.websocket.connect("ws://localhost:24892/custom?channel=settingSaveRequest")
-- script entrypoint
saveSettingSocket.OnMessage:connect(function(new_near_distance)
    if not tonumber(new_near_distance) then
        saveSettingSocket:Send("LETTER_ERROR")
    else
        NEAR_DISTANCE = tonumber(new_near_distance);
        print("NEW NEAR_DISTANCE: " .. new_near_distance)
    end
end)