<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
    xmlns:layer="LayerPrefabs"
    xmlns:physicist="PhysicistTypes"
    xmlns:paths="PhysicistPaths"
>
    <xsl:output method="xml" indent="yes"/>

  <xsl:param name="mapheight"/>

  <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>

  <xsl:template name="DefaultParameter">
    <xsl:param name="Input"/>
    <xsl:param name="DefaultValue"/>
    <xsl:param name="Type"/>
    <xsl:choose>
      <xsl:when test="$Input!=''">
        <xsl:choose>
          <xsl:when test="string($Type)='Attribute'">
            <xsl:value-of select="$Input"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:copy-of select="$Input"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="string($Type)='Attribute'">
            <xsl:value-of select="$DefaultValue"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:copy-of select="$DefaultValue"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="physicist:Map/layer:Elevator">
    <xsl:variable name="BoxID">
      <xsl:value-of select="generate-id(@boxTextureLocation)"/>
    </xsl:variable>
    <xsl:variable name="ShaftID">
      <xsl:value-of select="generate-id(@shaftTextureLocation)"/>
    </xsl:variable>
    <xsl:variable name="OverlayID">
      <xsl:value-of select="generate-id(@overlayTextureLocation)"/>
    </xsl:variable>
    <xsl:variable name="InteriorID">
      <xsl:value-of select="generate-id(@interiorTextureLocation)"/>
    </xsl:variable>
    <xsl:variable name="SuspensionID">
      <xsl:value-of select="generate-id(@suspensionTextureLocation)"/>
    </xsl:variable>
    <xsl:variable name="height">
      <xsl:number value="300 * @floorCount"/>
    </xsl:variable>

    <MapLayer xmlns="PhysicistTypes" name="{@name}" layerDepth="{@layerDepth}" width="78" height="{$height}" xoffset="{@xoffset}" yoffset="{@yoffset}">
      <Media>
        <Texture2D name="{$BoxID}" location="{@boxTextureLocation}"/>
        <Texture2D name="{$ShaftID}" location="{@shaftTextureLocation}"/>
        <Texture2D name="{$OverlayID}" location="{@overlayTextureLocation}"/>
        <Texture2D name="{$InteriorID}" location="{@interiorTextureLocation}"/>
        <Texture2D name="{$SuspensionID}" location="{@suspensionTextureLocation}"/>
        <xsl:copy-of select="layer:OtherMedia/*"/>
      </Media>

      <LevelObjects>
        <Backgrounds>
          <Backdrop name="{$ShaftID}" depth="0" textureRef="{$ShaftID}" tile="true">
            <Dimension height="{$height}" width="78"/>
            <Location x="0" y="{$height}"/>
          </Backdrop>
          <Backdrop name="{$SuspensionID}" depth="0.1" textureRef="{$SuspensionID}" tile="true">
            <Dimension height="{$height}" width="78"/>
            <Location x="0" y="{$height}"/>
          </Backdrop>
          <Backdrop name="{$OverlayID}" depth="1" textureRef="{$OverlayID}" tile="true">
            <Dimension height="{$height}" width="78"/>
            <Location x="0" y="{$height}"/>
          </Backdrop>
        </Backgrounds>

        <Actors>
          <Elevator startingFloor="{@startingFloor}" floorCount="{@floorCount}">
            <xsl:for-each select="layer:Floor">
              <xsl:copy-of select="current()"/>            
            </xsl:for-each>
            <xsl:call-template name="Actor_Template">
              <xsl:with-param name="BodyInfo">
                <BodyInfo>
                  <LoopShape bodyType="Kinematic" textureRef="ElevatorBox">
                    <Position x="0" y="{$height - 5 - (@startingFloor * 300)}"/>
                    <Vertices>
                      <Vector2 x="4" y="10"/>
                      <Vector2 x="74" y="10"/>
                      <Vector2 x="74" y="122"/>
                      <Vector2 x="4" y="122"/>
                    </Vertices>
                  </LoopShape>
                </BodyInfo>
              </xsl:with-param>

              <xsl:with-param name="GameSprites">
                <GameSprite spriteName="Box" depth="0.3" textureRef="{$BoxID}">
                  <FrameSize width="78" height="132"/>
                  <Offset x="0" y="0"/>
                  <Animations>
                    <Animation name="Idle" defaultFrameRate="1" frameCount="1"/>
                  </Animations>
                </GameSprite>
                <GameSprite spriteName="Interior" depth="0.35" textureRef="{$InteriorID}">
                  <FrameSize width="70" height="112"/>
                  <Offset x="4" y="10"/>
                  <Animations>
                    <Animation name="Idle" defaultFrameRate="1" frameCount="1"/>
                  </Animations>
                </GameSprite>
              </xsl:with-param>

              <xsl:with-param name="Health">
                <xsl:number value="10"/>
              </xsl:with-param>

              <xsl:with-param name="Name" select="@name"/>
            
              <xsl:with-param name="MovementSpeed">
                <MovementSpeed x="0" y="{@speed}"/>
              </xsl:with-param>

              <xsl:with-param name="Damage">
                <xsl:text disable-output-escaping="yes">false</xsl:text> 
              </xsl:with-param>

              <xsl:with-param name="Class">
                <xsl:text disable-output-escaping="yes">Elevator</xsl:text> 
              </xsl:with-param>

              <xsl:with-param name="PathManager">
                <PathManager>                
                  <xsl:for-each select="./layer:Floor">
                    <xsl:variable name="index" select="current()/@level"/>                 
                    <PhysicistPath isEnabled="false" name="{$index}" loopPath="false">
                      <paths:ApproachPositionPathNode speed="{../@speed}" precision="0">
                        <Position x="0" y="{$height - 5 - ($index * 300)}"/>
                      </paths:ApproachPositionPathNode>
                    </PhysicistPath>
                  </xsl:for-each>
                </PathManager>
              </xsl:with-param>
            </xsl:call-template>
          </Elevator>
          <xsl:copy-of select="layer:OtherActors/*"/>
        </Actors>

      </LevelObjects>

    </MapLayer>
  </xsl:template>

  <xsl:template name="Actor_Template">
    <xsl:param name="BodyInfo"/>
    <xsl:param name="Health"/>
    <xsl:param name="MovementSpeed"/>
    <xsl:param name="GameSprites"/>
    <xsl:param name="Damage"/>
    <xsl:param name="Class"/>
    <xsl:param name="PathManager"/>
    <xsl:param name="Name"/>
    <Actor class="{$Class}" name="{$Name}" health="{$Health}" rotation="0" isEnabled="true" visibleState="Visible" canBeDamaged="{$Damage}">
      <xsl:copy-of select="$MovementSpeed"/>
      <Sprites>
        <xsl:copy-of select="$GameSprites"/>
      </Sprites>
      <xsl:copy-of select="$BodyInfo"/>
      <xsl:copy-of select="$PathManager"/>
    </Actor>
  </xsl:template>

</xsl:stylesheet>
